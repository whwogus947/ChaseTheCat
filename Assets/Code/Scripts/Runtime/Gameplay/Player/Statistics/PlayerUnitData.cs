using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerUnitData : MonoBehaviour, IAbilities
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

    [System.Serializable]
    public class PlayerStat
    {
        public int maxHp;
        public int defense;
        public int normalDamage;
        public int closeDamage;
        public int rangedDamage;
        public int attackRPM;
        public int criticalDamage;
        public int criticalRate;
        public int maxStamina;
        public int recovery;
        public int avoidance;
        public int luck;
    }
}
