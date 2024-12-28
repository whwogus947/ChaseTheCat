using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerBehaviour : UnitBehaviour
    {
        public FloatValueSO direction;
        public BoolValueSO groundChecker;
        public BoolValueSO controllable;
        public VFXPool pool;
        public LayerMask enemyLayer;
        public LayerMask npcLayer;
        public VanishSlider jumpGauge;
        public AbilityController ability;
        public SkillViewGroup skillViewGroup;
        public PlayerStatSO playerStat;
        public VanishImage hpSlider;
        public VanishImage epSlider;
        public List<SkillAbilitySO> initialSkills;
        public Transform fxStorage;

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
        public float ep;

        public override bool Controllable { get => controllable.Value; set => controllable.Value = value; }

        private const string skillType = nameof(SkillAbilitySO);
        private AbilityContainer<SkillAbilitySO> Skills => ability.GetContainer<SkillAbilitySO>(skillType);
        private WeaponPlacer weaponPlacer;

        public void InvalidateRigidbody()
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        public void RegenerateRigidbody()
        {
            rb.gravityScale = 1f;
        }
        
        public override void UseVFX(PoolItem fx)
        {
            if (fx == null)
                return;

            var poolObj = pool.GetPooledObject(fx);
            if (poolObj.isFixed)
            {
                poolObj.transform.SetParent(fxStorage, false);
            }
            else
            {
                bool isFlip = GetFaceDirection() == 1;
                poolObj.transform.position = transform.position - fx.transform.position * GetFaceDirection();
                poolObj.GetComponent<SpriteRenderer>().flipX = isFlip;
            }
        }

        public void MeetupNPC()
        {
            var rayHit = Physics2D.BoxCast((Vector2)transform.position, Vector2.one, 0, FacingDirection * 1f * Vector2.right, 2, npcLayer.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out NPCBehaviour behaviour))
            {
                behaviour.OpenDialogue();
            }
            else if (rayHit.collider != null && rayHit.collider.TryGetComponent(out CatHairBall ball))
            {
                ball.InteractWithBall();
            }
        }

        public void ResetMaxHeight() => maxHeight = transform.position.y;

        public void CalculateFallDamage()
        {
            var endHeight = transform.position.y;
            var gap = endHeight - maxHeight;
            if (gap < threshold && groundChecker.Value)
            {
                PlayAnimation("main-hit", 0.2f);
                HP += (int)gap * power;
                maxHeight = transform.position.y;
            }
        }

        protected override void Initialize()
        {
            controllable.Value = true;
            var vanishImage = hpSlider;
            vanishImage.MaxValue = HP;
            vanishUI = vanishImage;
            epSlider.MaxValue = playerStat.maxEP;
            EP = 100;

            Skills.AddListener(AddSkill);

            initialSkills.ForEach(x => Skills.Add(x));
            weaponPlacer = GetComponent<WeaponPlacer>();
            weaponPlacer.fxEvent += UseVFX;
            weaponPlacer.onGetWeapon += OnGetWeapon;
            maxHeight = transform.position.y;
        }

        private void UpdateFallHeight()
        {
            if (transform.position.y > maxHeight)
                maxHeight = transform.position.y;
        }

        private void OnGetWeapon(WeaponAbilitySO weapon)
        {
            ability.AddAbility(weapon);
        }

        protected override void CheckPerFrame()
        {
            GroundCheck();
            if (IsCollideWithWall())
            {
                ToOppositeDirection();
            }

            UpdateAllSkills();
            EP += Time.deltaTime * playerStat.epRecoverySpeed;
            UpdateFallHeight();
        }

        private void UpdateAllSkills()
        {
            Skills?.Foreach(x => SkillUpdate(x));
        }

        private void SkillUpdate(SkillAbilitySO skill)
        {
            skill.CoolDown();
        }

        private void AddSkill(SkillAbilitySO skill)
        {
            skillViewGroup.AddSkill(skill);
        }

        protected override void Dead()
        {
            
        }

        protected override int GetVelocityDirection()
        {
            var velocity = direction.Value;
            int velocityDirection = velocity > 0 ? 1 : velocity < 0 ? -1 : 0;
            if (IsOppositeDirection(velocityDirection) && controllable.Value)
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
            var rayHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.92f, 0, Vector2.down, 20f, groundLayer.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            controllable.Value = groundChecker.Value = distance < 0.05f;
        }

        private bool IsCollideWithWall()
        {
            if (groundChecker.Value)
                return false;
            var rayHit = Physics2D.BoxCast((Vector2)transform.position + FacingDirection * 0.55f * Vector2.right, new Vector2(0.1f, 0.9f), 0, Vector2.zero, 0, groundLayer.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            return distance < 0.025f;
        }

        private void ToOppositeDirection()
        {
            unitImage.localScale = new(unitImage.localScale.x * -1, unitImage.localScale.y, 1);
            capturedDirection *= -1;
        }

        public override async void Attack()
        {
            weaponPlacer.AnimatorEvent(PlayAnimation);
            await weaponPlacer.Use();
            if (weaponPlacer.IsOffenseWeapon(out IOffensiveWeapon weapon))
            {
                weapon.Attack((Vector2)transform.position, FacingDirection * 1f * Vector2.right, enemyLayer, 0);
            }
        }
    }
}
