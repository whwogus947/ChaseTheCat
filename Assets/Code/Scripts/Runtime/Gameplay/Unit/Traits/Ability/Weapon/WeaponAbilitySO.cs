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
            weaponOnHand = Instantiate(weaponPrefab);
            weaponOnHand.transform.SetParent(_hand, false);
            weaponOnHand.SetActive(false);
            if (isLimited)
                this._count = count;
            OnAquire();
        }

        public void Equip()
        {
            weaponOnHand.SetActive(true);
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
// 
        // protected int Direction()
        // {
        //     Transform root = weaponOnHand.transform.root;
        //     var direction = (root.position - weaponOnHand.transform.position).x;
        //     return direction > 0 ? 1 : -1;
        // }
    }
}
