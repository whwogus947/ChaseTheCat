using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public abstract class WeaponAbilitySO : AbilitySO, IWeapon
    {
        public override string AbilityType => nameof(WeaponAbilitySO);
        public bool isRightHanded;
        public Sprite frame;
        public OffensiveWeapon Entity => weaponOnHand;
        public bool IsLimited => _count.HasValue;
        public UnityAction<int> onCountChanged;
        public int Count
        {
            get => _count ?? 0;
            set
            {
                _count = value;
                if (_count.HasValue)
                {
                    Debug.Log(_count);
                    onCountChanged((int)_count);
                }
            }
        }

        [SerializeField] private OffensiveWeapon weaponPrefab;
        protected OffensiveWeapon weaponOnHand;

        private int? _count = null;

        public void Obtain(Transform _hand)
        {
            _count = null;
            onCountChanged = delegate { };
            weaponOnHand = Instantiate(weaponPrefab);
            weaponOnHand.transform.SetParent(_hand, false);
            weaponOnHand.gameObject.SetActive(false);

            if (this is ICountable countable)
            {
                _count = countable.InitialCount;
            }
            OnAquire();
        }

        public void Equip()
        {
            weaponOnHand.gameObject.SetActive(true);
        }

        protected void TakeOne()
        {
            if (_count.HasValue)
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

        private bool IsWeaponReadyToUse() => Entity.IsReady;

        public abstract void OnUseWeapon();
    }
}
