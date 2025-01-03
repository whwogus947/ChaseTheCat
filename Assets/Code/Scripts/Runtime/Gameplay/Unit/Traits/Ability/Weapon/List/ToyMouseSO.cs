using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Toy Mouse", menuName = "Cum2usGameDev/Ability/Weapon/List/ToyMouse")]
    public class ToyMouseSO : WeaponAbilitySO, ICountable
    {
        public override string AbilityName => nameof(ToyMouseSO);

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
