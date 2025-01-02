using UnityEngine;

namespace Com2usGameDev
{
    public class Projectile : PoolItem
    {
        public AudioClip hitSound;

        private int damage;
        private float speed;
        private Vector2 flyDirection;
        private LayerMask targetLayer;
        private float timer;

        public void Initialize(ProjectileSettings settings, Vector3 from, Vector2 direction, LayersSO layer)
        {
            transform.position = from;
            speed = settings.flySpeed;
            flyDirection = direction;
            targetLayer = layer.target.value;
            timer = settings.lifeTime;
            damage = settings.damage;
            GetComponentInChildren<SpriteRenderer>().flipX = direction.x > 0;
        }

        void Update()
        {
            Attack(transform.position, flyDirection, targetLayer);
            transform.Translate(speed * Time.deltaTime * flyDirection);

            timer -= Time.deltaTime;
            if (timer < 0)
            {
                onReturnToPool(this);
            }
        }

        private void Attack(Vector2 from, Vector2 to, LayerMask layer)
        {
            var rayHit = Physics2D.CircleCast(from, 0.5f, to, 0.25f, layer.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out UnitBehaviour behaviour))
            {
                // audioChannel.Invoke(hitSound);
                behaviour.HP -= damage;
                onReturnToPool(this);
            }
        }
    }

    [System.Serializable]
    public class ProjectileSettings
    {
        public Projectile prefab;
        public int damage;
        public float flySpeed;
        public float lifeTime;
    }
}
