using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Toy Mouse", menuName = "Cum2usGameDev/Ability/Weapon/List/ToyMouse")]
    public class ToyMouseSO : WeaponAbilitySO
    {
        public override int AnimationHash => Animator.StringToHash("main-attack");
        public override string AbilityName => nameof(ToyMouseSO);
        public int initialCount;

        private Throwable toyMouse;

        public override void OnAquire()
        {
            Count = initialCount;
        }

        public override void UseWeapon()
        {
            if (toyMouse == null)
                toyMouse = weaponOnHand.GetComponent<Throwable>();
            
            TakeOne();
        }
    }
}
