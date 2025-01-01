using UnityEngine;

namespace Com2usGameDev
{
    public interface IOffensiveWeapon
    {
        void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage);
    }

    public abstract class OffensiveWeapon : MonoBehaviour, IOffensiveWeapon
    {
        [ReadOnly] public AudioChannelSO audioChannel;
        [Header("Basic Stats")]
        public float delay = 0.3f;
        public float range = 2.2f;
        [Header("Settings")]
        public AudioClip attackSound;
        public int AnimationHash => _animationHash ??= Animator.StringToHash(animationName);
        
        [SerializeField] private string animationName;
        private int? _animationHash;

        public abstract void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage);
    }
}
