using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Slingshot", menuName = "Cum2usGameDev/Ability/Weapon/List/Slingshot")]
    public class SlingshotSO : WeaponAbility
    {
        public override string AbilityName => nameof(SlingshotSO);
        public override int AnimationHash => Animator.StringToHash("main-attack2");

        private SlingshotController slingshotController;

        public override void OnAquire()
        {
            
        }

        public override void UseWeapon()
        {
            if (slingshotController == null)
                slingshotController = weaponOnHand.GetComponent<SlingshotController>();
            
            slingshotController.LineDrawingTimer = 0.688f;
        }
    }
}
