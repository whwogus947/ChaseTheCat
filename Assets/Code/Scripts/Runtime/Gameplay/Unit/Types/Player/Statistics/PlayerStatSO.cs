using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Player Stat", menuName = "Cum2usGameDev/Unit/Player/Stat")]
    public class PlayerStatSO : UnitStatSO
    {
        public int maxEP;
        public float epRecoverySpeed;
        public Vector2 jumpPower;
        public float jumpCharge;
    }
}
