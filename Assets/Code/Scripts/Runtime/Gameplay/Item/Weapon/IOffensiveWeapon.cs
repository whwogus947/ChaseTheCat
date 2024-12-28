using UnityEngine;

namespace Com2usGameDev
{
    public interface IOffensiveWeapon
    {
        void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage);
    }

    public abstract class OffensiveWeapon : MonoBehaviour, IOffensiveWeapon
    {
        public float delay = 0.3f;
        public float detectRange = 4f;
        public abstract void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage);
    }
}
