using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class UnitBehaviour : MonoBehaviour
    {
        public LinearStatSO direction;
        public BoolValueSO groundChecker;
        public Transform unitImage;
        public BoolValueSO controllable;
        public LayerMask groundLayer;
        public float walk;
        public float run;
        public float jump;
        public float jumpX;
        public float dash;
        public float jumpCharging;

        private Animator ani;
        private Rigidbody2D rb;
        private int UnitDirection => GetDirection();
        private float transitionPower;
        private int capturedDirection;

        private void Awake()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            controllable.Value = true;
        }

        void Update()
        {
            GroundCheck();
        }

        public void SetTransitionPower(float power) => transitionPower = power;

        public void TranslateX()
        {
            rb.linearVelocityX = UnitDirection * transitionPower;
        }

        public void TranslateFixedX()
        {
            rb.linearVelocityX = capturedDirection * transitionPower;
        }

        public void Jump()
        {
            rb.AddForceY(jump);
        }

        public void PlayAnimation(int animHash, float transitionRate = 0f)
        {
            if (transitionRate > 0)
            {
                ani.CrossFade(animHash, transitionRate);
                return;
            }
            ani.Play(animHash);
        }

        public void SetAnimation(string animHash, bool value)
        {
            ani.SetBool(animHash, value);
        }

        public void CaptureDirection(float? value = null)
        {
            SetTransitionPower(value == null ? transitionPower : (float)value);
            capturedDirection = GetDirection();
        }

        private void GroundCheck()
        {
            var rayHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.96f, 0, Vector2.down, 20f, groundLayer.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            groundChecker.Value = distance < 0.05f;
        }

        private int GetDirection()
        {
            int directionValue = (int)direction.value;
            if ((directionValue == TransformToInt() * -1) && controllable.Value)
            {
                unitImage.localScale = new(unitImage.localScale.x * -1, 1, 1);
            }
            return directionValue;
        }

        private int TransformToInt()
        {
            return unitImage.localScale.x > 0 ? -1 : 1;
        }
    }
}
