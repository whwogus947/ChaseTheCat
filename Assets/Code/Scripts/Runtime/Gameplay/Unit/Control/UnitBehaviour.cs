using UnityEngine;

namespace Com2usGameDev
{
    public abstract class UnitBehaviour : Sonorous
    {
        public LayerMask groundLayer;
        public float walk;
        public float run;
        public float jump;
        public float jumpX;
        public float dash;
        public float jumpCharging;
        public float chargePower;
        public abstract bool Controllable {get; set;}
        public CountdownTimer timer;
        public float hp;
        public AudioClip attackSound;

        protected IVanishable vanishUI;
        public float HP
        {
            get => hp;
            set
            {
                hp = value;
                vanishUI.SetValue(hp);
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

        public abstract void UseVFX(PoolItem fx);

        protected abstract void Initialize();

        protected abstract void CheckPerFrame();

        protected abstract int GetVelocityDirection();

        public void SetTransitionPower(float power) => transitionPower = power;

        public void TranslateX()
        {
            if (Controllable)
                rb.linearVelocityX = VelocityDirection * transitionPower;
        }

        public void TranslateFixedX()
        {
            rb.linearVelocityX = capturedDirection * transitionPower;
        }

        public void Jump() => rb.AddForceY(jump * Mathf.Clamp(chargePower, 0, 1));

        public void PlayAnimation(int animHash, float transitionRate = 0f)
        {
            if (transitionRate > 0)
            {
                ani.CrossFade(animHash, transitionRate);
                return;
            }
            ani.Play(animHash);
        }

        protected void PlayAnimation(string animName, float transitionRate = 0f)
        {
            ani.CrossFade(animName, transitionRate);
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
