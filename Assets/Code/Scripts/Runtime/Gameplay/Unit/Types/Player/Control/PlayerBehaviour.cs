using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerBehaviour : UnitBehaviour, IAbilityBundle<SkillAbilitySO>, IAbilityBundle<WeaponAbilitySO>, ISpawnable
    {
        public TransformChannelSO playerLocator;
        public AbilityController abilityController;

        [Header("Stats")]
        public PlayerStatSO playerStat;

        [Header("VFX")]
        public VFXPool pool;
        public PlayerVFX VFXList;
        public Bloodscreen bloodscreen;
        public Transform fxStorage;

        [Header("SFX")]
        public PlayerSFX SFXList;
        public float ChargePower { get; set; }
        public readonly ControlData controlData = new();

        private float maxHeight;
        private readonly float threshold = -7.5f;
        private readonly int power = 2;

        public float EP
        {
            get => ep;
            set
            {
                ep = Mathf.Clamp(value, 0, playerStat.maxEP);
                epSlider.SetValue(ep);
            }
        }
        private float ep;

        public override bool Controllable { get => controlData.controllable; set => controlData.controllable = value; }

        public VanishSlider jumpGauge => playerView.jumpGauge;
        public VanishImage hpSlider => playerView.hpSlider;
        public VanishImage epSlider => playerView.epSlider;

        private WeaponHandler weaponHandler;
        private SkillHandler skillHandler;

        //************************************************************************************************************

        public InputControllerSO inputController;

        public AbilityController Controller => abilityController;

        AbilityViewGroup<WeaponAbilitySO> IAbilityBundle<WeaponAbilitySO>.ViewGroup
            => gameObject.GetComponentInEntire<WeaponViewGroup>();

        AbilityHolder<WeaponAbilitySO> IAbilityBundle<WeaponAbilitySO>.Holder
            => gameObject.GetComponentInEntire<WeaponHolder>();

        AbilityViewGroup<SkillAbilitySO> IAbilityBundle<SkillAbilitySO>.ViewGroup
            => gameObject.GetComponentInEntire<SkillViewGroup>();

        AbilityHolder<SkillAbilitySO> IAbilityBundle<SkillAbilitySO>.Holder
            => gameObject.GetComponentInEntire<SkillHolder>();

        public void InvalidateRigidbody()
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        public float Delay => weaponHandler.WeaponDelay;

        public override UnitStatSO Stat => playerStat;

        public void RestartRigidbody() => rb.gravityScale = 1f;

        public void ResetVelocity() => rb.linearVelocity = Vector2.zero;

        protected override UnitView View { get => playerView; set => playerView = value as PlayerView; }

        private PlayerView playerView;

        public override void VisualizeFX(PoolItem fx) => pool.Visualize(fx, fxStorage, GetFaceDirection());

        public void Interaction()
        {
            var rayHit = Physics2D.BoxCast((Vector2)transform.position, Vector2.one, 0, FacingDirection * 1f * Vector2.right, 2, layerData.npc.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out IInteractable interactee))
            {
                interactee.Interact();
            }
        }

        public void ResetMaxHeight() => maxHeight = float.MinValue;

        public void CalculateFallDamage()
        {
            var endHeight = transform.position.y;
            var gap = endHeight - maxHeight;
            if (gap < threshold && controlData.groundValue)
            {
                PlayAnimation("main-hit", 0.2f);
                HP += (int)gap * power;
                maxHeight = transform.position.y;
            }
        }

        protected override void Initialize()
        {
            View = gameObject.GetComponentInEntire<PlayerView>();

            playerLocator.UniqueEvent(PlacePlayerToStartLocation);

            var vanishImage = hpSlider;
            vanishImage.MaxValue = Stat.maxHP;
            vanishUI = vanishImage;
            epSlider.MaxValue = playerStat.maxEP;
            EP = 100;

            maxHeight = transform.position.y;


            var input = inputController.GetOrCreate();

            weaponHandler = new WeaponHandler(this, input, Hands);
            weaponHandler.fxEvent += VisualizeFX;

            skillHandler = new SkillHandler(this);
        }

        private void PlacePlayerToStartLocation(Transform placer)
        {
            ResetMaxHeight();
            rb.bodyType = RigidbodyType2D.Dynamic;
            transform.position = placer.position;
            rb.linearVelocity = Vector2.zero;
        }

        private void UpdateFallHeight()
        {
            if (transform.position.y > maxHeight)
                maxHeight = transform.position.y;
        }

        protected override void CheckPerFrame()
        {
            GroundCheck();
            if (IsCollideWithWall())
            {
                ToOppositeDirection();
            }

            skillHandler.UpdateAllSkills();
            EP += Time.deltaTime * playerStat.epRecoverySpeed;
            UpdateFallHeight();
        }

        protected override void Dead()
        {

        }

        protected override int GetVelocityDirection()
        {
            var velocity = controlData.velocityDirection;
            int velocityDirection = velocity > 0 ? 1 : velocity < 0 ? -1 : 0;
            if (IsOppositeDirection(velocityDirection) && controlData.controllable)
            {
                FacingDirection = velocityDirection;
            }
            return velocityDirection;
        }

        private bool IsOppositeDirection(int direction)
        {
            return direction == FacingDirection * -1;
        }

        private void GroundCheck()
        {
            var rayHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.92f, 0, Vector2.down, 20f, layerData.ground.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            controlData.controllable = controlData.groundValue = distance < 0.05f;
        }

        private bool IsCollideWithWall()
        {
            if (controlData.groundValue)
                return false;
            var rayHit = Physics2D.BoxCast((Vector2)transform.position + FacingDirection * 0.55f * Vector2.right, new Vector2(0.1f, 0.9f), 0, Vector2.zero, 0, layerData.ground.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;

                var fx = pool.GetPooledObject(VFXList.wall);
                fx.transform.position = rayHit.point;
                if (fx is TwinklePoolItem item)
                {
                    item.StartTwinkle();
                }
                else
                {
                    fx.gameObject.SetActive(false);
                }
            }
            return distance < 0.025f;
        }

        private void ToOppositeDirection()
        {
            unitImage.localScale = new(unitImage.localScale.x * -1, unitImage.localScale.y, 1);
            capturedDirection *= -1;
        }

        public override void Attack()
        {
            weaponHandler.Activate(WeaponInfo, PlayAnimation);
        }

        private WeaponPerformInfo WeaponInfo => new(
            (Vector2)transform.position,
            FacingDirection * 1f * Vector2.right,
            layerData,
            0);

        protected override void OnLowHP()
        {
            base.OnLowHP();
            if (HP <= 25 && !bloodscreen.gameObject.activeSelf)
            {
                bloodscreen.gameObject.SetActive(true);
            }
            else if (HP > 25 && bloodscreen.gameObject.activeSelf)
            {
                bloodscreen.gameObject.SetActive(false);
            }
        }

        public void Jump() => rb.AddForceY(playerStat.jumpPower.y * Mathf.Clamp(ChargePower, 0.4f, 1));

        public void Jump(float power) => rb.AddForceY(playerStat.jumpPower.y * power);

        public void Spawn(MapSpawner spawner)
        {
            
        }
    }

    [System.Serializable]
    public class PlayerSFX
    {
        public AudioClip dash;
        public AudioClip staticFlight;
        public AudioClip jump;
        public AudioClip doubleJump;
    }

    [System.Serializable]
    public class PlayerVFX
    {
        [Header("Collision")]
        public TwinklePoolItem monster;
        public TwinklePoolItem wall;
    }
}
