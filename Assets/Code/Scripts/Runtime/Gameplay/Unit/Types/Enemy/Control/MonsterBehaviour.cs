using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class MonsterBehaviour : UnitBehaviour, ISpawnable
    {
        public LayerMask playerLayer;
        public OffensiveWeapon weapon;
        private Transform player;
        public float detectRange = 4f;
        public override bool Controllable { get => enabled; set => enabled = value; }
        public EnemyStatSO enemyStat;
        public override UnitStatSO Stat => enemyStat;

        public GameObject Spawnable => gameObject;

        private MaterialPropertyBlock propertyBlock;
        private Renderer[] renderers;
        private bool isDissolveOn = false;
        private bool isAttacking = false;
        private VibrateEffector onFindPlayerEffect;

        public override void VisualizeFX(PoolItem fx)
        {

        }

        public void ChangeDirectionToOpposite()
        {
            FacingDirection *= -1;
        }

        protected override void Initialize()
        {
            propertyBlock = new MaterialPropertyBlock();
            renderers = GetComponentsInChildren<Renderer>();
            vanishUI = GetComponentInChildren<VanishSlider>();
            onFindPlayerEffect = GetComponentInChildren<VibrateEffector>(true);
        }

        protected override void CheckPerFrame()
        {

        }

        public void OnFindPlayer()
        {
            if (onFindPlayerEffect != null && !onFindPlayerEffect.gameObject.activeSelf)
            {
                onFindPlayerEffect.StartEffect(audioChannel);
            }
        }

        public void UpdateAllRendererProperties(float value)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.GetPropertyBlock(propertyBlock);

                propertyBlock.SetFloat("_Dissolve", value);

                renderer.SetPropertyBlock(propertyBlock);
            }
        }

        protected override void Dead()
        {
            if (isDissolveOn)
                return;

            GetComponent<Collider2D>().enabled = false;

            isDissolveOn = true;
            DissolveRoutine().Forget();
            Controllable = false;
            vanishUI.OnFadeaway();
        }

        private async UniTaskVoid DissolveRoutine()
        {
            float alpha = 0;
            while (alpha <= 1)
            {
                alpha += Time.deltaTime * 1f;
                UpdateAllRendererProperties(alpha);
                await UniTask.Yield();
            }
            UpdateAllRendererProperties(1);
            Destroy(gameObject);
        }

        public void ResetVelocityDirection() => player = null;

        protected override int GetVelocityDirection()
        {
            return FacingDirection = player != null ? GetTargetDirection(player.position) : FacingDirection;
        }

        public bool IsOnGround()
        {
            bool isChasable = IsGround(Vector3.right * GetVelocityDirection());
            if (!isChasable)
                ChangeDirectionToOpposite();
            return isChasable;
        }

        private int GetTargetDirection(Vector3 target)
        {
            return target.x > transform.position.x ? 1 : -1;
        }

        private bool IsGround(Vector3 relativePos)
        {
            var cols = Physics2D.OverlapBox(transform.position + Vector3.down * 0.5f + relativePos, Vector2.one * 0.3f, 0f, layerData.ground.value);
            return cols != null;
        }

        public bool IsPlayerNearby(float? size = null)
        {
            float detectSizeX = size == null ? 16 : (float)size;
            var cols = Physics2D.OverlapBox(transform.position, new Vector2(detectSizeX, 0.45f), 0f, playerLayer.value);
            if (player == null && cols != null)
                player = cols.transform;

            return cols != null;
        }

        public bool IsPlayerBeside(float? size = null)
        {
            float detectSizeX = size == null ? 4 : (float)size;
            var cols = Physics2D.OverlapBox(transform.position, new Vector2(detectSizeX, 0.45f), 0f, playerLayer.value);
            if (player == null && cols != null)
                player = cols.transform;

            return cols != null && DistanceX(player.position.x) <= detectRange;
        }

        private float DistanceX(float playerX)
        {
            // return Mathf.Abs(playerX - transform.position.x);
            return Vector2.Distance(player.position, transform.position);
        }

        public override async void Attack()
        {
            if (weapon == null || isAttacking)
                return;

            isAttacking = true;
            PlayAnimation(weapon.AnimationHash, 0.2f);
            weapon.Use(transform.position, FacingDirection * Vector2.right, layerData, 0).Forget();
            await UniTask.WaitForSeconds(weapon.delay);
            if (this == null)
                return;

            isAttacking = false;
        }
    }
}
