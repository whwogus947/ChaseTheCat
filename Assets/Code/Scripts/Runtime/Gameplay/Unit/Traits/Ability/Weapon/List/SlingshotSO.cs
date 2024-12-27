using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Slingshot", menuName = "Cum2usGameDev/Ability/Weapon/List/Slingshot")]
    public class SlingshotSO : WeaponAbilitySO
    {
        public override string AbilityName => nameof(SlingshotSO);
        public override int AnimationHash => Animator.StringToHash("main-attack2");
        
        [SerializeField] private float fireTiming = 0.688f;

        private SlingshotController slingshotController;

        public override void OnAquire()
        {

        }

        public override void UseWeapon()
        {
            if (slingshotController == null)
                slingshotController = weaponOnHand.GetComponent<SlingshotController>();

            fxDelay = fireTiming;
            slingshotController.LineDrawingTimer = fireTiming;
        }
    }
}
