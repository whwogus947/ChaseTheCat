
using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerBehaviour : UnitBehaviour
    {
        public LinearStatSO direction;
        public BoolValueSO groundChecker;
        public BoolValueSO controllable;
        public VFXPool pool;
        public LayerMask enemyLayer;

        public override bool Controllable { get => controllable.Value; set => controllable.Value = value; }

        
        public override void UseVFX()
        {
            var poolObj = pool.GetPooledObject();
            bool isFlip = GetFaceDirection() == 1;
            poolObj.GetComponent<SpriteRenderer>().flipX = isFlip;
        }

        protected override void Initialize()
        {
            controllable.Value = true;
        }

        protected override void CheckPerFrame()
        {
            GroundCheck();
            if (IsCollideWithWall())
            {
                ToOppositeDirection();
            }
        }

        protected override void Dead()
        {
            
        }

        protected override int GetVelocityDirection()
        {
            var velocity = direction.value;
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
