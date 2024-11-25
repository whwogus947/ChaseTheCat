using UnityEngine;

namespace Com2usGameDev
{
    public abstract class UnitBehaviour : MonoBehaviour
    {
        public ClampedVector2dSO direction;
        public UnitStatistics stat;

        [Header("Unit States")]
        public UnitStatInformationSO idleState;
        public UnitStatInformationSO walkState;
        public UnitStatInformationSO runState;
        public UnitStatInformationSO jumpState;
        public UnitStatInformationSO jumpChargingState;

        protected Rigidbody2D rb;
        protected float prevVelocityX;
        protected Animator animator;
        protected Transform child;
        

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            child = transform.GetChild(0);
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
