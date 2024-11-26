using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Linear Stat", menuName = "Cum2usGameDev/Statistics/LinearStat")]
    public class LinearStatSO : ScriptableObject
    {
        public float value;
        public float recoveryPerSecond;

        public bool TryUse(float amount)
        {
            amount *= Time.deltaTime;
            if (value <= amount)
            {
                value = 0;
                return false;
            }

            value -= amount;
            return true;
        }

        public void RecoverPerFrame(float frameTime)
        {
            value += recoveryPerSecond * frameTime;
        }
    }
}
