using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Com2usGameDev
{
    public class WeaponPlacer : MonoBehaviour
    {
        public Transform leftHand;
        public Transform rightHand;
        public GameObject vfxStorage;
        public List<WeaponAbilitySO> initialWeapons;
        public UnityAction<PoolItem> fxEvent;
        public UnityAction<WeaponAbilitySO> onGetWeapon;
        public WeaponViewGroup weaponViewGroup;
        public AbilityController ability;

        private PCInput input;

        private readonly List<WeaponAbilitySO> weapons = new();
        private WeaponAbilitySO currentWeapon;
        private const string weaponType = nameof(WeaponAbilitySO);
        private AbilityContainer<WeaponAbilitySO> Weapons => ability.GetContainer<WeaponAbilitySO>(weaponType);


        void Start()
        {
            Weapons.AddListener(OnAddWeapon);

            initialWeapons.ForEach(AddWeapon);
            if (weapons.Count > 0)
                Replace(weapons[0]);

            input = GetComponent<UserInputHandler>().inputController.GetOrCreate();
            input.Player.NextWeapon.performed += OnNextWeaponClicked;
            input.Player.BeforeWeapon.performed += OnBeforeWeaponClicked;
        }

        private void OnBeforeWeaponClicked(InputAction.CallbackContext context)
        {
            Replace(GetCircular(+1));
        }

        private void OnNextWeaponClicked(InputAction.CallbackContext context)
        {
            Replace(GetCircular(-1));
        }

        private WeaponAbilitySO GetCircular(int param)
        {
            int idx = weapons.FindIndex(x => x == currentWeapon) + param;
            int circularIndex = (idx % weapons.Count + weapons.Count) % weapons.Count;
            return weapons[circularIndex];
        }

        private void OnAddWeapon(WeaponAbilitySO weapon)
        {
            weaponViewGroup.AddWeapon(weapon);
            if (weapon.isLimited)
            {
                weapon.onCountChanged += (int count) =>
                {
                    if (count <= 0)
                        PutDown(true);
                };
            }
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

        public void AddWeapon(WeaponAbilitySO weapon)
        {
            if (!weapons.Contains(weapon))
            {
                Transform parent = weapon.isRightHanded ? rightHand : leftHand;
                weapon.Obtain(parent);
                weapons.Add(weapon);
                Debug.Log(onGetWeapon);
                Debug.Log(weapon);
                Debug.Log(ability);
                onGetWeapon(weapon);
            }
        }

        public void Replace(WeaponAbilitySO weapon)
        {
            if (!weapons.Contains(weapon))
            {
                AddWeapon(weapon);
                return;
            }
            PutDown();
            Equip(weapon);
        }

        private void PutDown(bool swap = false)
        {
            if (currentWeapon == null)
            {
                return;
            }
            currentWeapon.Unequip();
            if (swap && weapons.Count > 1)
            {
                int idx = weapons.FindIndex(x => x == currentWeapon);
                int length = weapons.Count - 1;
                weapons.RemoveAt(idx);
                Equip(weapons[idx & length]);
            }
        }

        private void Equip(WeaponAbilitySO weapon)
        {
            currentWeapon = weapon;
            weapon.Equip();
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
