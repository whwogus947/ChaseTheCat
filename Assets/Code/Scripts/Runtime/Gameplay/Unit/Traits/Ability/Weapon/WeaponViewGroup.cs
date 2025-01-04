using UnityEngine;

namespace Com2usGameDev
{
    public class WeaponViewGroup : AbilityViewGroup<WeaponAbilitySO>
    {
        public Transform storage;

        private WeaponSlot slot;

        void Start()
        {
            slot = storage.GetComponentInChildren<WeaponSlot>(true);
        }

        public override void AddAbility(WeaponAbilitySO skill)
        {
            WeaponSlot newSlot = null;
            for (int i = 0; i < storage.childCount; i++)
            {
                if (!storage.GetChild(i).gameObject.activeSelf)
                {
                    var targetSlot = storage.GetChild(i);
                    newSlot = targetSlot.GetComponent<WeaponSlot>();
                    break;
                }
            }

            if (newSlot == null)
                newSlot = Instantiate(slot, storage);

            newSlot.gameObject.SetActive(true);
            newSlot.SetWeapon(skill);
        }
    }
}
