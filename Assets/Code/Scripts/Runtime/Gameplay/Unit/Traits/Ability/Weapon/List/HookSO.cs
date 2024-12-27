using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Hook", menuName = "Cum2usGameDev/Ability/Weapon/List/Hook")]
    public class HookSO : WeaponAbility
    {
        public override int AnimationHash => Animator.StringToHash("main-hook");
        public override string AbilityName => nameof(HookSO);
        public bool IsUsing { get; set; } = false;

        private HookController hookController;

        public override void OnAquire()
        {
            IsUsing = false;
        }

        public override void UseWeapon()
        {
            if (hookController == null)
                hookController = weaponOnHand.GetComponent<HookController>();

            Debug.Log("Use Rope!");
            IsUsing = true;
            hookController.CastRope(() => IsUsing = false);
        }
    }
}
