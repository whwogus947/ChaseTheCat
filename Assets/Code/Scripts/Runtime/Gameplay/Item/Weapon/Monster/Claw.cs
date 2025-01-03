using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class Claw : OffensiveWeapon
    {
        [Header("FX")]
        public PoolItem attackFx;

        public async override UniTask Use(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            PlaySound();
            var rayHit = Physics2D.BoxCast(from, Vector2.one, 0, to, range, layer.target.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out PlayerBehaviour behaviour))
            {
                behaviour.HP -= 10;
                var fx = fXPool.GetPooledObject(attackFx);
                fx.transform.position = (Vector2)transform.position + to * 0.55f * Vector2.right;
                var scale = fx.transform.localScale;
                int direction = to.x > 0 ? -1 : 1;
                fx.transform.localScale = new(Mathf.Abs(scale.x) * direction, scale.y, scale.z);
            }
        }
    }
}
