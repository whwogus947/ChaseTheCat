using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Hook", menuName = "Cum2usGameDev/Ability/Weapon/List/Hook")]
    public class HookSO : WeaponAbility
    {
        public override int AnimationHash => Animator.StringToHash("main-hook");
        public override string AbilityName => nameof(HookSO);

        private HookController hookController;

        public override void OnAquire()
        {

        }

        public override void UseWeapon()
        {
            if (hookController == null)
                hookController = weaponOnHand.GetComponent<HookController>();

            hookController.CastRope();
        }
    }
}
