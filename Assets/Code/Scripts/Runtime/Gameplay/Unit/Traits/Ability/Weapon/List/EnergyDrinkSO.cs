using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Energy Drink", menuName = "Cum2usGameDev/Ability/Weapon/List/EnergyDrink")]
    public class EnergyDrinkSO : WeaponAbilitySO
    {
        public override int AnimationHash => throw new System.NotImplementedException();

        public override string AbilityName => throw new System.NotImplementedException();

        public override void OnAquire()
        {
            
        }

        public override void UseWeapon()
        {
            throw new System.NotImplementedException();
        }
    }
}
