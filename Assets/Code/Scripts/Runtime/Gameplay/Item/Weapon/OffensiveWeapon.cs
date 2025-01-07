using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class OffensiveWeapon : MonoBehaviour
    {
        [ReadOnly] public AudioChannelSO audioChannel;
        [ReadOnly] public VFXPool fXPool;
        [Header("Basic Stats")]
        public int damage;
        public float delay = 0.3f;
        public float range = 2.2f;

        [Header("Settings")]
        [SerializeField] private AnimationClipNameSO animationClipName;
        [SerializeField] private AudioClip weaponSound;

        public int AnimationHash => animationClipName.Hash;
        public bool IsReady { get; private set; } = true;

        public async UniTaskVoid TryUse(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            if (!IsReady)
                return;
            
            IsReady = false;
            await Use(from, to, layer, defaultDamage);
            IsReady = true;
        }

        public abstract UniTask Use(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage);

        protected void PlaySound(AudioClip clip = null)
        {
            if (clip == null)
                clip = weaponSound;
            audioChannel.Invoke(clip);
        }
    }
}
