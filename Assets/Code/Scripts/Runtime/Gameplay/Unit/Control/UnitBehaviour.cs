using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public abstract class UnitBehaviour : MonoBehaviour
    {
        public LayerMask groundLayer;
        public float walk;
        public float run;
        public float jump;
        public float jumpX;
        public float dash;
        public float jumpCharging;
        public abstract bool Controllable {get; set;}
        public CountdownTimer timer;
        public float hp;

        public VanishSlider slider;
        public float HP
        {
            get => hp;
            set
            {
                hp = value;
                slider.SetValue(hp);
                if (hp <= 0)
                {
                    Dead();
                }
            }
        }

        protected Transform unitImage;
        protected int capturedDirection;
        protected int FacingDirection
        {
            get => GetFaceDirection();
            set => IntToTransform(value);
        }
        protected Rigidbody2D rb;
        private Animator ani;
        private int VelocityDirection => GetVelocityDirection();
        private float transitionPower;

        private void Awake()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            timer = new(0);
            unitImage = transform.GetChild(0);
            Initialize();
            HP = hp;
        }

        void Update()
        {
            CheckPerFrame();            
        }

        public abstract void Attack();

        protected abstract void Dead();

        public abstract void UseVFX();

        protected abstract void Initialize();

        protected abstract void CheckPerFrame();

        protected abstract int GetVelocityDirection();

        public void SetTransitionPower(float power) => transitionPower = power;

        public void TranslateX()
        {
            rb.linearVelocityX = VelocityDirection * transitionPower;
        }

        public void TranslateFixedX()
        {
            rb.linearVelocityX = capturedDirection * transitionPower;
        }

        public void Jump() => rb.AddForceY(jump);

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

        protected int GetFaceDirection()
        {
            return unitImage.localScale.x > 0 ? -1 : 1;
        }

        private void IntToTransform(int value)
        {
            unitImage.localScale = new(Mathf.Abs(unitImage.localScale.x) * value * -1, unitImage.localScale.y, 1);
        }
    }
}
