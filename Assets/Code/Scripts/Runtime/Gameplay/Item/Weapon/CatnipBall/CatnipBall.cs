using UnityEngine;

namespace Com2usGameDev
{
    public class CatnipBall : OffensiveWeapon
    {
        public LayerMask attackLayer;
        public LayerMask groundLayer;
        public Vector2Int throwPower;
        public AudioClip onHitSfx;

        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;

        public override void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            sfxChannel?.Invoke(sfx);
            var clone = Instantiate(this, transform.position, transform.rotation);
            clone.transform.SetParent(null, true);
            clone.transform.localScale = Vector3.one;
            clone.rb.bodyType = RigidbodyType2D.Dynamic;
            clone.rb.AddForce(to * throwPower.x + Vector2.up * throwPower.y);
            clone.circleCollider.enabled = true;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = false;
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (!circleCollider.enabled)
                return;

            var col = Physics2D.OverlapCircle(transform.position, 1f, attackLayer.value);
            if (col != null && col.TryGetComponent(out UnitBehaviour behaviour))
            {
                sfxChannel.Invoke(onHitSfx);
                behaviour.HP -= 30;
                Destroy(gameObject);
            }

            var groundCol = Physics2D.OverlapCircle(transform.position, 0.25f, groundLayer.value);
            if (groundCol != null)
            {
                circleCollider.isTrigger = false;
                Destroy(gameObject, 5f);
            }
        }
    }
}
