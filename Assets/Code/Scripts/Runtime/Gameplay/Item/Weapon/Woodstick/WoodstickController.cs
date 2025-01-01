using UnityEngine;

namespace Com2usGameDev
{
    public class WoodstickController : OffensiveWeapon
    {
        public AudioClip sfx;

        public override void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            audioChannel.Invoke(sfx);
            
            var rayHit = Physics2D.BoxCast(from, Vector2.one, 0, to, 5, layer);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out MonsterBehaviour behaviour))
            {
                behaviour.HP -= 40;
            }
        }
    }
}
