using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class WeaponPlacer : MonoBehaviour
    {
        public Transform hand;
        public GameObject vfxStorage;
        public List<WeaponAbility> initialWeapons;
        public UnityAction<PoolItem> fxEvent;

        private readonly List<WeaponAbility> weapons = new();
        private WeaponAbility currentWeapon;

        void Start()
        {
            initialWeapons.ForEach(AddWeapon);
            if (weapons.Count > 0)
                Replace(weapons[0]);
        }

        public void AddWeapon(WeaponAbility weapon)
        {
            if (!weapons.Contains(weapon))
            {
                weapon.Obtain(hand.gameObject, vfxStorage);
                weapons.Add(weapon);
            }
        }

        public void Replace(WeaponAbility weapon)
        {
            if (!weapons.Contains(weapon))
            {
                AddWeapon(weapon);
                return;
            }
            PutDown();
            currentWeapon = weapon;
            weapon.Equip();
        }

        private void PutDown()
        {
            if (currentWeapon == null)
            {
                return;
            }
            currentWeapon.Unequip();
        }

        public void Use()
        {
            if (currentWeapon != null)
            {
                Debug.Log("Use Weapon");
                currentWeapon.UseWeapon();
                if (currentWeapon.fx != null)
                {
                    if (currentWeapon.fxDelay > 0)
                        FXRoutine().Forget();
                    else
                        fxEvent?.Invoke(currentWeapon.fx);
                }
            }
        }

        private async UniTaskVoid FXRoutine()
        {
            await UniTask.WaitForSeconds(currentWeapon.fxDelay);
            fxEvent?.Invoke(currentWeapon.fx);
        }
    }
}
