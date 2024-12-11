using UnityEngine;

namespace Com2usGameDev
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
        public VFXPool pool;

        private Animator ani;
        private Rigidbody2D rb;
        private int UnitDirection => GetVelocityDirection();
        private float transitionPower;
        private int capturedDirection;
        protected int CharacterDirection
        {
            get => GetFaceDirection();
            set => IntToTransform(value);
        }

        private void Awake()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            controllable.Value = true;
            Initialize();
        }

        void Update()
        {
            GroundCheck();
            if (IsCollideWithWall())
            {
                ToOppositeDirection();
            }
        }

        protected virtual void Initialize()
        {

        }

        public void UseVFX()
        {
            var poolObj = pool.GetPooledObject();
            bool isFlip = GetFaceDirection() == 1 ? true : false;
            poolObj.GetComponent<SpriteRenderer>().flipX = isFlip;
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
            capturedDirection = GetVelocityDirection();
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
            var rayHit = Physics2D.BoxCast((Vector2)transform.position + CharacterDirection * 0.55f * Vector2.right, new Vector2(0.1f, 0.9f), 0, Vector2.zero, 0, groundLayer.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            return distance < 0.025f;
        }

        protected virtual int GetVelocityDirection()
        {
            var velocity = direction.value;
            int velocityDirection = velocity > 0 ? 1 : velocity < 0 ? -1 : 0;
            if (IsOppositeDirection(velocityDirection) && controllable.Value)
            {
                CharacterDirection = velocityDirection;
            }
            return velocityDirection;
        }

        private void ToOppositeDirection()
        {
            unitImage.localScale = new(unitImage.localScale.x * -1, unitImage.localScale.y, 1);
            capturedDirection *= -1;
        }

        private bool IsOppositeDirection(int direction)
        {
            return direction == CharacterDirection * -1;
        }

        private int GetFaceDirection()
        {
            return unitImage.localScale.x > 0 ? -1 : 1;
        }

        private void IntToTransform(int value)
        {
            unitImage.localScale = new(Mathf.Abs(unitImage.localScale.x) * value * -1, unitImage.localScale.y, 1);
        }
    }
}
