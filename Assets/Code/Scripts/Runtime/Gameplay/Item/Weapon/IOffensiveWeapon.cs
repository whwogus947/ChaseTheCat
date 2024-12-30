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
        public float attackRange = 2.2f;
        public abstract void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage);
        public string animationName;
        public AudioChannelSO sfxChannel;
        public AudioClip sfx;
    }
}
