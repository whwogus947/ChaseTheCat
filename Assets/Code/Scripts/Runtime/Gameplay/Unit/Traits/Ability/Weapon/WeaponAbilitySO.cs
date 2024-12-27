using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public abstract class WeaponAbilitySO : AbilitySO, IWeapon
    {
        public override string AbilityType => nameof(WeaponAbilitySO);
        public abstract int AnimationHash { get; }
        public PoolItem fx;
        public float fxDelay = 0.3f;
        public bool isRightHanded;
        public GameObject Entity => weaponOnHand;
        public Sprite frame;
        public bool isLimited;
        public UnityAction<int> onCountChanged;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                onCountChanged(_count);
            }
        }

        [SerializeField] private GameObject weaponPrefab;
        protected GameObject weaponOnHand;

        private int _count;

        public void Obtain(Transform _hand, int count = 1)
        {
            onCountChanged = delegate { };
            weaponOnHand = Instantiate(weaponPrefab, _hand);
            weaponOnHand.SetActive(false);
            if (isLimited)
                this._count = count;
        }

        public void Equip()
        {
            weaponOnHand.SetActive(true);
            OnAquire();
        }

        protected void TakeOne()
        {
            Count -= 1;
        }

        public void Unequip()
        {
            weaponOnHand.SetActive(false);
        }

        public abstract void UseWeapon();
    }
}
