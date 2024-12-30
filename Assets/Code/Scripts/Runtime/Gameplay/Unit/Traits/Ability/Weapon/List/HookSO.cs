using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Hook", menuName = "Cum2usGameDev/Ability/Weapon/List/Hook")]
    public class HookSO : WeaponAbilitySO
    {
        public override int AnimationHash => Animator.StringToHash("main-hook");
        public override string AbilityName => nameof(HookSO);
        public bool IsUsing { get; set; } = false;
        public int initialCount;

        private HookController hookController;

        public override void OnAquire()
        {
            IsUsing = false;
            Count = initialCount;
        }

        public override void OnDiscover()
        {
            Count = initialCount;
        }

        public override void UseWeapon()
        {
            if (hookController == null)
                hookController = weaponOnHand.GetComponent<HookController>();

            IsUsing = true;
            hookController.CastRope(OnAfterUseRope);
        }

        private void OnAfterUseRope()
        {
            TakeOne();
            IsUsing = false;
        }
    }
}
