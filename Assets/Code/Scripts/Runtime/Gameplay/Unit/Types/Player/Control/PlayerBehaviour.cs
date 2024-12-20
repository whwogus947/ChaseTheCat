
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
        public SkillAbilitySO dashSkill;
        public SkillViewGroup skillViewGroup;

        public override bool Controllable { get => controllable.Value; set => controllable.Value = value; }

        private const string skillType = nameof(SkillAbilitySO);
        private AbilityContainer<SkillAbilitySO> Skills => ability.GetContainer<SkillAbilitySO>(skillType);

        public void InvalidateRigidbody()
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        public void RegenerateRigidbody()
        {
            rb.gravityScale = 1f;
        }
        
        public override void UseVFX()
        {
            var poolObj = pool.GetPooledObject();
            bool isFlip = GetFaceDirection() == 1;
            poolObj.GetComponent<SpriteRenderer>().flipX = isFlip;
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

        protected override void Initialize()
        {
            controllable.Value = true;
            var vanishImage = GetComponentInChildren<VanishImage>();
            vanishImage.maxValue = HP;
            vanishUI = vanishImage;
        }

        protected override void CheckPerFrame()
        {
            GroundCheck();
            if (IsCollideWithWall())
            {
                ToOppositeDirection();
            }

            UpdateAllSkills();
        }

        private void UpdateAllSkills()
        {
            Skills.Foreach(x => SkillUpdate(x));
        }

        private void SkillUpdate(SkillAbilitySO skill)
        {
            skill.CoolDown();
        }

        public void AddSkill(SkillAbilitySO skill)
        {
            Skills.Add(skill);
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

        public override void Attack()
        {
            var rayHit = Physics2D.BoxCast((Vector2)transform.position, Vector2.one, 0, FacingDirection * 1f * Vector2.right, 5, enemyLayer.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out MonsterBehaviour behaviour))
            {
                behaviour.HP -= 40;
            }
        }
    }
}
