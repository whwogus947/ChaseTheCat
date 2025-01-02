using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Animation Clip Name", menuName = "Cum2usGameDev/Core/Animation/ClipName")]
    public class AnimationClipNameSO : ScriptableObject
    {
        public int Hash
        {
            get
            {
                _hash ??= Animator.StringToHash(clipName);
                return (int)_hash;
            }
        }

        private int? _hash = null;
        [SerializeField] private string clipName;
    }
}
