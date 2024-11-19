using UnityEngine;
using UnityEngine.UIElements;

namespace Com2usGameDev
{
    [RequireComponent(typeof(UIDocument))]
    public class PlayerUnitData : MonoBehaviour, IAbilities
    {
        public LinearStatSO hpBar;
        public LinearStatSO epBar;

        private UIDocument document;

        void Start()
        {
            document = GetComponent<UIDocument>();
            var root = document.rootVisualElement;
            var epElement = root.Q<ProgressBar>("ep-bar");
            var hpElement = root.Q<ProgressBar>("hp-bar");
            hpElement.style.backgroundColor = Color.blue;
            hpElement.dataSource = hpBar;
            epElement.dataSource = epBar;
        }

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
