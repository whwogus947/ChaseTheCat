using UnityEngine;

namespace Com2usGameDev
{
    public class Throwable : ThrowableWeapon
    {
        [Header("Spin")]
        public bool spin = false;
        public float spinSpeed = 180;

        private Transform spinTransform;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = false;
        }

        void Start()
        {
            if (spin && transform.childCount < 1)
                spin = false;
            else if (spin)
                spinTransform = transform.GetChild(0);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (spin)
                spinTransform.Rotate(Vector3.forward, spinSpeed * direction * Time.deltaTime);
        }

        protected override void OnCollisionGround()
        {
            var groundCol = Physics2D.OverlapCircle(transform.position, 0.25f, layers.ground.value);
            if (groundCol != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
