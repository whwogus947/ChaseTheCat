using UnityEngine;

namespace Com2usGameDev
{
    public interface IOffensiveWeapon
    {
        void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage);
    }
}
