using UnityEngine;

namespace Com2usGameDev
{
    public class ThrowableWeapon : OffensiveWeapon
    {
        public AudioClip hitSound;

        [Header("Throwable")]
        public Vector2Int throwPower;

        protected LayersSO layers;
        protected Rigidbody2D rb;
        protected CircleCollider2D circleCollider;
        protected int direction;

        private int playerDamage;

        public override void Use(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            PlaySound();
            var clone = Instantiate(this, transform.position, transform.rotation);
            clone.playerDamage = defaultDamage;
            clone.direction = to.x > 0 ? -1 : 1;
            Initiate(clone, to);
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = false;
        }

        private void Initiate(ThrowableWeapon clone, Vector2 throwDirection)
        {
            clone.transform.SetParent(null, true);
            clone.transform.localScale = Vector3.one;
            clone.rb.bodyType = RigidbodyType2D.Dynamic;
            clone.rb.AddForce(throwDirection * throwPower.x + Vector2.up * throwPower.y);
            clone.circleCollider.enabled = true;
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                sprite.flipX = throwDirection.x > 0;
            }
        }

        void Update()
        {
            if (!circleCollider.enabled)
                return;

            OnCollisionEnemy();
            OnCollisionGround();
            OnUpdate();
        }

        protected virtual void OnCollisionEnemy()
        {
            var col = Physics2D.OverlapCircle(transform.position, 1f, layers.target.value);
            if (col != null && col.TryGetComponent(out UnitBehaviour behaviour))
            {
                PlaySound(hitSound);
                behaviour.HP -= damage + playerDamage;
                Destroy(gameObject);
            }
        }

        protected virtual void OnCollisionGround()
        {

        }

        protected virtual void OnUpdate()
        {

        }
    }
}
