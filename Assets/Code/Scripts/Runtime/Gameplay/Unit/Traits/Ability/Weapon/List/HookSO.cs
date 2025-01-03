using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Hook", menuName = "Cum2usGameDev/Ability/Weapon/List/Hook")]
    public class HookSO : WeaponAbilitySO, ICountable
    {
        public override string AbilityName => nameof(HookSO);
        // public bool IsUsing { get; set; } = false;
        public int InitialCount { get => initialCount; }

        [SerializeField] private int initialCount = 1;

        // private HookController hookController;

        public override void OnAquire()
        {
            // IsUsing = false;
            // Count = initialCount;
        }

        // public override void OnDiscover()
        // {
        //     Count = initialCount;
        // }

        public override void OnUseWeapon()
        {
            // if (hookController == null)
            //     hookController = weaponOnHand.GetComponent<HookController>();

            // // IsUsing = true;
            // hookController.CastRope(OnAfterUseRope);
        }

        private void OnAfterUseRope()
        {
            // TakeOne();
            // IsUsing = false;
        }
    }
}
