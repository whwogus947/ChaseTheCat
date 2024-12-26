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

        private readonly List<WeaponAbility> weapons = new();
        private WeaponAbility currentWeapon;

        void Start()
        {
            initialWeapons.ForEach(AddWeapon);
            if (weapons.Count > 0)
                Replace(weapons[0]);
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
