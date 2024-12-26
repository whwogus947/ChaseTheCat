using UnityEngine;

namespace Com2usGameDev
{
    public abstract class WeaponAbility : AbilitySO, IWeapon
    {
        public override string AbilityType => nameof(WeaponAbility);
        public abstract int AnimationHash { get; }
        public PoolItem fx;
        public float fxDelay = 0.3f;
        public bool isRightHanded;

        [SerializeField] private GameObject weaponPrefab;
        // [SerializeField] private GameObject FX;
        private GameObject weaponOnHand;
        // protected GameObject fxClone;

        public void Obtain(Transform _hand)
        {
            weaponOnHand = Instantiate(weaponPrefab, _hand);
            weaponOnHand.SetActive(false);
        }

        public void Equip()
        {
            weaponOnHand.SetActive(true);
        }

        public void Unequip()
        {
            weaponOnHand.SetActive(false);
        }

        public abstract void UseWeapon();
    }
}
