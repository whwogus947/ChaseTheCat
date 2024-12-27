using UnityEngine;

namespace Com2usGameDev
{
    public class CatnipBall : MonoBehaviour, IOffensiveWeapon
    {
        public LayerMask monsterLayer;
        public LayerMask groundLayer;
        public Vector2Int throwPower;

        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;

        public void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            var clone = Instantiate(this, transform.position, transform.rotation);
            clone.transform.SetParent(null, true);
            clone.transform.localScale = Vector3.one;
            clone.rb.bodyType = RigidbodyType2D.Dynamic;
            clone.rb.AddForce(to * throwPower.x + Vector2.up * throwPower.y);
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
        }

        void Start()
        {

        }

        void Update()
        {
            var col = Physics2D.OverlapCircle(transform.position, 1f, monsterLayer.value);
            if (col != null && col.TryGetComponent(out MonsterBehaviour monsterBehaviour))
            {
                monsterBehaviour.HP -= 30;
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
