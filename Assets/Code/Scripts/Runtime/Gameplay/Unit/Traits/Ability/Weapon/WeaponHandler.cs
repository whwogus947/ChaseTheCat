using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Com2usGameDev
{
    public class WeaponHandler : AbilityHandler<WeaponAbilitySO>
    {
        public UnityAction<PoolItem> fxEvent;

        public float WeaponDelay => currentAbility.Entity.delay;
        private readonly Hands hands;

        public WeaponHandler(IAbilityBundle<WeaponAbilitySO> bundle, PCInput inputActions, Hands handData) : base(bundle)
        {
            hands = handData;
            inputActions.Player.NextWeapon.performed += OnNextWeaponClicked;
            inputActions.Player.BeforeWeapon.performed += OnBeforeWeaponClicked;
            Ability.AddListener(OnAddWeapon);
            AddInitialAbilities(bundle.Holder.initialItems);
        }

        public async void Activate(WeaponPerformInfo info, UnityAction<int, float> animation)
        {
            if (currentAbility == null)
                return;

            animation(AnimationHash, 0.2f);
            currentAbility.Use().Forget();
            if (IsOffenseWeapon(out OffensiveWeapon weapon))
            {
                bool result = await weapon.TryUse(info.from, info.to, info.layers, info.damage);
            }
            // if (currentWeapon.fxDelay > 0)
            // await UniTask.WaitForSeconds(WeaponDelay);

            // if (currentWeapon.fx != null)
            // fxEvent?.Invoke(currentWeapon.fx);
        }

        private void OnGetWeapon(WeaponAbilitySO weapon)
        {
            controller.AddAbility(weapon);
        }

        protected override void AddInitialAbilities(List<WeaponAbilitySO> initialWeapons)
        {
            initialWeapons.ForEach(AddWeapon);
            if (abilities.Count > 0)
                Replace(abilities[0]);
        }

        private void OnBeforeWeaponClicked(InputAction.CallbackContext context) => Replace(GetCircular(+1));

        private void OnNextWeaponClicked(InputAction.CallbackContext context) => Replace(GetCircular(-1));
        
        private WeaponAbilitySO GetCircular(int param)
        {
            int idx = abilities.FindIndex(x => x == currentAbility) + param;
            int circularIndex = (idx % abilities.Count + abilities.Count) % abilities.Count;
            return abilities[circularIndex];
        }

        private void OnAddWeapon(WeaponAbilitySO weapon)
        {
            viewGroup.AddAbility(weapon);
            if (weapon.IsLimited)
            {
                weapon.onCountChanged += (int count) =>
                {
                    if (count <= 0)
                        PutDown(true);
                };
            }
        }

        private bool IsOffenseWeapon(out OffensiveWeapon offensiveWeapon)
        {
            if (currentAbility.Entity.TryGetComponent(out OffensiveWeapon weapon))
            {
                offensiveWeapon = weapon;
                return true;
            }
            offensiveWeapon = null;
            return false;
        }

        private int AnimationHash => currentAbility.Entity.AnimationHash;

        public void AddWeapon(WeaponAbilitySO weapon)
        {
            if (!abilities.Contains(weapon))
            {
                Transform parent = weapon.isRightHanded ? hands.right : hands.left;
                weapon.Obtain(parent);
                abilities.Add(weapon);
                OnGetWeapon(weapon);
            }
        }

        private void Replace(WeaponAbilitySO weapon)
        {
            if (!abilities.Contains(weapon))
            {
                AddWeapon(weapon);
                return;
            }
            PutDown();
            Equip(weapon);
        }

        private void PutDown(bool swap = false)
        {
            if (currentAbility == null)
            {
                return;
            }
            currentAbility.Unequip();
            if (swap && abilities.Count > 1)
            {
                int idx = abilities.FindIndex(x => x == currentAbility);
                abilities.RemoveAt(idx);
                int length = abilities.Count - 1;
                Equip(abilities[Mathf.Clamp(idx, 0, length)]);
            }
        }

        private void Equip(WeaponAbilitySO weapon)
        {
            currentAbility = weapon;
            weapon.Equip();
        }
    }
}
