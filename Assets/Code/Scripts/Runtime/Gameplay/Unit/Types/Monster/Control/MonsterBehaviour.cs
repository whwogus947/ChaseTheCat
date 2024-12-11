using UnityEngine;

namespace Com2usGameDev
{
    public class MonsterBehaviour : UnitBehaviour
    {
        public LayerMask playerLayer;
        private Transform player;

        protected override void Initialize()
        {
            base.Initialize();
            // playerLayer = LayerMask.NameToLayer("Player");
        }

        protected override int GetVelocityDirection()
        {
            return CharacterDirection = player != null ? GetTargetDirection(player.position) : CharacterDirection;
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
    }
}
