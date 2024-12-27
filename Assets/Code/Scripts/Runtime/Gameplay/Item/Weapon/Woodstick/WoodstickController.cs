using UnityEngine;

namespace Com2usGameDev
{
    public class WoodstickController : MonoBehaviour, IOffensiveWeapon
    {
        public void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            var rayHit = Physics2D.BoxCast(from, Vector2.one, 0, to, 5, layer);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out MonsterBehaviour behaviour))
            {
                behaviour.HP -= 40;
            }
        }
    }
}
