using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class SlingshotBullet : MonoBehaviour, IOffensiveWeapon
    {
        public int damage = 40;
        public AudioChannelSO audioChannel;
        public AudioClip sfx;

        private float speed;
        private Vector2 flyDirection;
        private LayerMask enemyLayer;
        private UnityAction<SlingshotBullet> onReturnPool;
        private float timer;
        private UnityAction onHit;

        public void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            var rayHit = Physics2D.CircleCast(from, 0.5f, to, 0.25f, layer.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out UnitBehaviour behaviour))
            {
                audioChannel.Invoke(sfx);
                behaviour.HP -= damage + defaultDamage;
                onReturnPool(this);
                onHit();
            }
        }

        public void AddHitEvent(UnityAction @event)
        {
            onHit = @event;
        }

        public void Initialize(float speed, Vector2 direction, LayerMask layer, float lifeTime, UnityAction<SlingshotBullet> onReturnPool)
        {
            this.speed = speed;
            flyDirection = direction;
            enemyLayer = layer;
            this.onReturnPool = onReturnPool;
            timer = lifeTime;
        }

        void Update()
        {
            Attack(transform.position, flyDirection, enemyLayer, 0);
            transform.Translate(speed * Time.deltaTime * flyDirection);

            timer -= Time.deltaTime;
            if (timer < 0)
            {
                onReturnPool(this);
            }
        }
    }
}
