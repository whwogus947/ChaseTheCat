using UnityEngine;

namespace Com2usGameDev
{
    public class MonsterBehaviour : UnitBehaviour
    {
        public LayerMask playerLayer;
        public float attackRange;

        private Transform player;

        public override bool Controllable { get => enabled; set => enabled = value; }

        public override void UseVFX()
        {
            
        }

        protected override void Initialize()
        {
            
        }

        protected override void CheckPerFrame()
        {
            
        }

        protected override int GetVelocityDirection()
        {
            return FacingDirection = player != null ? GetTargetDirection(player.position) : FacingDirection;
        }

        public bool IsChasable()
        {
            return IsGround(Vector3.right * GetVelocityDirection());
        }

        public bool IsMovable(int direction)
        {
            return IsGround(Vector3.right * direction);
        }

        private int GetTargetDirection(Vector3 target)
        {
            return target.x > transform.position.x ? 1 : -1;
        }

        private bool IsGround(Vector3 relativePos)
        {
            var cols = Physics2D.OverlapBox(transform.position + Vector3.down + relativePos, Vector2.one, 0f, groundLayer.value);
            return cols != null;
        }

        public bool IsPlayerNearby()
        {
            var cols = Physics2D.OverlapBox(transform.position, new Vector2(8, 2), 0f, playerLayer.value);
            if (player == null && cols != null)
                player = cols.transform;

            return cols != null;
        }

        public bool IsPlayerBeside()
        {
            var cols = Physics2D.OverlapBox(transform.position, new Vector2(4, 2), 0f, playerLayer.value);
            if (player == null && cols != null)
                player = cols.transform;

            return cols != null&& DistanceX(player.position.x) <= attackRange;
        }

        private float DistanceX(float playerX)
        {
            return Mathf.Abs(playerX - transform.position.x);
        }
    }
}
