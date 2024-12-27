using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class WeaponPlacer : MonoBehaviour
    {
        public Transform leftHand;
        public Transform rightHand;
        public GameObject vfxStorage;
        public List<WeaponAbility> initialWeapons;
        public UnityAction<PoolItem> fxEvent;
        public UnityAction<WeaponAbility> onGetWeapon;

        private readonly List<WeaponAbility> weapons = new();
        private WeaponAbility currentWeapon;

        void Start()
        {
            initialWeapons.ForEach(AddWeapon);
            if (weapons.Count > 0)
                Replace(weapons[0]);
        }

        public bool IsOffenseWeapon(out IOffensiveWeapon offensiveWeapon)
        {
            if (currentWeapon.Entity.TryGetComponent(out IOffensiveWeapon weapon))
            {
                offensiveWeapon = weapon;
                return true;
            }
            offensiveWeapon = null;
            return false;
        }

        public void AnimatorEvent(UnityAction<int, float> animationHash)
        {
            animationHash?.Invoke(currentWeapon.AnimationHash, 0.2f);
        }

        public void AddWeapon(WeaponAbility weapon)
        {
            if (!weapons.Contains(weapon))
            {
                Transform parent = weapon.isRightHanded ? rightHand : leftHand;
                weapon.Obtain(parent);
                weapons.Add(weapon);
                onGetWeapon(weapon);
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

        public async UniTask Use()
        {
            if (currentWeapon == null)
                return;

            currentWeapon.UseWeapon();
            if (currentWeapon.fxDelay > 0)
                await UniTask.WaitForSeconds(currentWeapon.fxDelay);

            if (currentWeapon.fx != null)
                fxEvent?.Invoke(currentWeapon.fx);
        }
    }
}
