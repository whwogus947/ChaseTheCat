using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Unit Stat Information", menuName = "Cum2usGameDev/Unit/Stat Information")]
    public class UnitStatInformationSO : ScriptableObject
    {
        public ClampedVector2dSO direction;
        [field: SerializeField] public string AnimationHashName { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public float EPDeltaConsume { get; private set; }
        [field: SerializeField] public AnimationClip Clip { get; private set; }
    }
}
