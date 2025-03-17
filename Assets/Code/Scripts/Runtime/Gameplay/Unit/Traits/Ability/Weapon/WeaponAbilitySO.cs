using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public abstract class WeaponAbilitySO : AbilitySO
    {
        public override string AbilityTypeName => nameof(WeaponAbilitySO);
        public override Type AbilityType => typeof(WeaponAbilitySO);
        public bool isRightHanded;
        public Sprite frame;
        public OffensiveWeapon Entity => weaponOnHand;
        public bool IsLimited => this is ICountable;
        public UnityAction<int> onCountChanged;
        public int Count
        {
            get => savableData.count.HasValue ? (int)savableData.count : 1;
            set
            {
                savableData.count = value;
                if (savableData.count.HasValue)
                {
                    // Debug.Log(savableData.count);
                    onCountChanged((int)savableData.count);
                }
            }
        }

        protected OffensiveWeapon weaponOnHand;

        [SerializeField] private OffensiveWeapon weaponPrefab;
        private SavableWeaponData savableData;

        // private int? _count = null;

        public void Obtain(Transform _hand)
        {
            // _count = null;
            Debug.Log("Obtained!");
            savableData = new(AbilityType, ID, null);
            onCountChanged = delegate { };
            weaponOnHand = Instantiate(weaponPrefab);
            weaponOnHand.transform.SetParent(_hand, false);
            weaponOnHand.gameObject.SetActive(false);

            if (this is ICountable countable)
            {
                // _count = countable.InitialCount;
                savableData.count = countable.InitialCount;
            }
            OnAquire();
        }

        public void Equip()
        {
            weaponOnHand.gameObject.SetActive(true);
        }

        protected void TakeOne()
        {
            // if (_count.HasValue)
            if (savableData.count.HasValue)
                Count -= 1;
        }

        public void Unequip()
        {
            weaponOnHand.gameObject.SetActive(false);
        }

        public async UniTaskVoid Use()
        {
            await UniTask.WaitForSeconds(Entity.delay);
            await UniTask.WaitUntil(IsWeaponReadyToUse);
            TakeOne();
            OnUseWeapon();
        }

        public abstract void OnUseWeapon();

        public override void ToSaveData(BookData book)
        {
            book.ToSaveData(savableData);
        }

        public override void ConvertDataToInstance(SavableProperty data)
        {
            if (this is ICountable)
            {
                Count = (int)(data as SavableWeaponData).count;
            }
        }

        private bool IsWeaponReadyToUse() => Entity.IsReady;
    }
}
