using UnityEngine;

namespace Com2usGameDev
{
    public abstract class UnitStatSO : ScriptableObject
    {
        public string unitName;
        public int maxHP;
        public int attackDamage;
        public float walkSpeed;
        public float runSpeed;
        public float attackRange;
        public float attackSpeed;
    }
}
