using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Catnip Ball", menuName = "Cum2usGameDev/Ability/Weapon/List/CatnipBall")]
    public class CatnipBallSO : WeaponAbilitySO, ICountable
    {
        public override string AbilityName => nameof(CatnipBallSO);
        public int InitialCount { get => initialCount; }

        [SerializeField] private int initialCount = 1;

        public override void OnAquire()
        {
            
        }

        public override void OnUseWeapon()
        {
        }
    }
}
