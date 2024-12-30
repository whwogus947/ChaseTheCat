using UnityEngine;

namespace Com2usGameDev
{
    public class ToyMouse : OffensiveWeapon
    {
        public int damage = 30;
        public LayerMask attackLayer;
        public LayerMask groundLayer;
        public Vector2Int throwPower;
        public bool spin = false;
        public float spinSpeed = 180;

        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;
        private Transform spinTransform;
        private int direction;

        public override void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            var clone = Instantiate(this, transform.position, transform.rotation);
            clone.transform.SetParent(null, true);
            clone.transform.localScale = Vector3.one;
            clone.rb.bodyType = RigidbodyType2D.Dynamic;
            clone.rb.AddForce(to * throwPower.x + Vector2.up * throwPower.y);
            clone.circleCollider.enabled = true;
            clone.direction = to.x > 0 ? -1 : 1;
        }

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

        void Update()
        {
            if (!circleCollider.enabled)
                return;

            if (spin)
                spinTransform.Rotate(Vector3.forward, spinSpeed * direction * Time.deltaTime);

            var col = Physics2D.OverlapCircle(transform.position, 0.1f, attackLayer.value);
            if (col != null && col.TryGetComponent(out UnitBehaviour behaviour))
            {
                behaviour.HP -= damage;
                Destroy(gameObject);
            }

            var groundCol = Physics2D.OverlapCircle(transform.position, 0.25f, groundLayer.value);
            if (groundCol != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
