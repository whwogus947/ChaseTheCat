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
        public GameObject Entity => weaponOnHand;

        [SerializeField] private GameObject weaponPrefab;
        protected GameObject weaponOnHand;

        public void Obtain(Transform _hand)
        {
            weaponOnHand = Instantiate(weaponPrefab, _hand);
            weaponOnHand.SetActive(false);
        }

        public void Equip()
        {
            weaponOnHand.SetActive(true);
            OnAquire();
        }

        public void Unequip()
        {
            weaponOnHand.SetActive(false);
        }

        public abstract void UseWeapon();
    }
}
