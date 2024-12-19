using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Volume Handler", menuName = "Cum2usGameDev/UI/VolumeHandler")]
    public class VolumeHandlerSO : ScriptableObject
    {
        private float Value;
        public UnityAction<float> onVolumeChange;

        public void HandlingVolume(float value)
        {
            Value = value;
            onVolumeChange?.Invoke(Value);
        }
    }
}
