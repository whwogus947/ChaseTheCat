using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Energy Drink", menuName = "Cum2usGameDev/Ability/Weapon/List/EnergyDrink")]
    public class EnergyDrinkSO : WeaponAbilitySO, ICountable
    {
        public override string AbilityName => throw new System.NotImplementedException();
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
