using UnityEngine;

namespace Com2usGameDev
{
    public abstract class WeaponAbility : AbilitySO, IWeapon
    {
        public override string AbilityType => nameof(WeaponAbility);
        public PoolItem fx;
        public float fxDelay = 0.3f;

        [SerializeField] private GameObject weaponPrefab;
        // [SerializeField] private GameObject FX;
        private GameObject weaponOnHand;
        // protected GameObject fxClone;

        public void Obtain(GameObject _hand, GameObject _vfxStorage)
        {
            weaponOnHand = Instantiate(weaponPrefab, _hand.transform);
            // fxClone = Instantiate(FX, _vfxStorage.transform);
            
            weaponOnHand.SetActive(false);
            // fxClone.SetActive(false);
        }

        public void Equip()
        {
            weaponOnHand.SetActive(true);
            // fxClone.SetActive(false);
        }

        public void Unequip()
        {
            weaponOnHand.SetActive(false);
        }

        public abstract void UseWeapon();
    }
}
