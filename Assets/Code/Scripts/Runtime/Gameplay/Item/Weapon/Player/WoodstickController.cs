using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class WoodstickController : OffensiveWeapon
    {
        public async override UniTask Use(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            PlaySound();
            
            var rayHit = Physics2D.BoxCast(from, Vector2.one, 0, to, 5, layer.target.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out MonsterBehaviour behaviour))
            {
                behaviour.HP -= damage;
            }
            await UniTask.WaitForSeconds(0.2f);
        }
    }
}
