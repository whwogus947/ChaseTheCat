using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Com2usGameDev
{
    [RequireComponent(typeof(UIDocument))]
    public class PlayerUnitData : MonoBehaviour, IAbilities
    {
        public InputControllerSO controller;
        public LinearStatSO hpBar;
        public LinearStatSO epBar;
        public LinearStatSO hpDeltaBar;

        private UIDocument document;

        void Start()
        {
            document = GetComponent<UIDocument>();
            var root = document.rootVisualElement;
            var epElement = root.Q<ProgressBar>("ep-bar");
            var hpElement = root.Q<ProgressBar>("hp-bar");
            var hpDelta = root.Q<ProgressBar>("hp-delta");
            hpElement.dataSource = hpBar;
            epElement.dataSource = epBar;
            hpDelta.dataSource = hpDeltaBar;
            hpBar.value = 100;
            hpDelta.value = 100;

            controller.Input.Player.Test.performed += OnTest;
        }

        private void OnTest(InputAction.CallbackContext context)
        {
            Debug.Log("On TEST!!!");
            hpBar.value -= UnityEngine.Random.Range(1, 15f);
        }

        void Update()
        {
            hpDeltaBar.value = Mathf.Lerp(hpDeltaBar.value, hpBar.value, Time.deltaTime * 3f);
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
