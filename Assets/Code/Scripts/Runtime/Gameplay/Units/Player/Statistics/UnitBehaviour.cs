using UnityEngine;

namespace Com2usGameDev
{
    public abstract class UnitBehaviour : MonoBehaviour
    {
        public ClampedVector2dSO direction;
        public UnitStatistics stat;
        public LinearStatSO epDeltaBar;

        [Header("Unit States")]
        public UnitStatInformationSO idleState;
        public UnitStatInformationSO walkState;
        public UnitStatInformationSO runState;
        public UnitStatInformationSO jumpState;
        public UnitStatInformationSO dashState;
        public UnitStatInformationSO jumpChargingState;
        public UnitStatInformationSO normalAttackState;

        protected Rigidbody2D rb;
        protected Animator animator;
        protected Transform child;
        

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            child = transform.GetChild(0);
        }

        public int ChildDirection()
        {
            return (int)child.localScale.x > 0 ? -1 : 1;
        }

        public bool HasEnoughEP(float value)
        {
            var leftover = stat.ep - value * Time.deltaTime;
            if (leftover > 0)
            {
                stat.ep = leftover;
                return true;
            }
            return false;
        }

        public void OnUpdate()
        {
            MoveX(direction.X * direction.power);
        }

        public abstract void MoveX(float power);

        public abstract void Jump(float power);

        public void PlayAnimation(int hash)
        {
            animator.CrossFade(hash, 0.15f);
        }

        public void SetAnimationBool(string trigger, bool value)
        {
            animator.SetBool(trigger, value);
        }

        public bool IsCollideWithWall()
        {
            if (IsOnGround())
                return false;
            var rayHit = Physics2D.BoxCast(transform.position, Vector2.one * 1f, 0, Vector2.zero, 0, direction.groundLayer.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            return distance < 0.025f;
        }

        private bool IsOnGround()
        {
            var rayHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.92f, 0, Vector2.down, 20f, direction.groundLayer.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            return distance < 0.11f;
        }
    }

    [System.Serializable]
    public class UnitStatistics
    {
        public float speed;
        public float damage;
        public float hp;
        public float ep;
    }
}
