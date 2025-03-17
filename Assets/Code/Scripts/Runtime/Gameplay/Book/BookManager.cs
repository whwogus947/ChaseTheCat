using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class BookManager : MonoBehaviour
    {
        public AbilityController controller;
        public Transform slotStorage;

        [Header("Button By Type")]
        public Button itmeButton;
        public Button monsterButton;
        public Button npcButton;

        private readonly List<BookSlot> slots = new();
        private int tempIndex = 0;
        
        void Start()
        {
            Initialize();

            itmeButton.onClick.AddListener(OpenTagedBoock<WeaponAbilitySO>);
            monsterButton.onClick.AddListener(OpenTagedBoock<MonsterAbilitySO>);
            npcButton.onClick.AddListener(OpenTagedBoock<NPCAbilitySO>);
        }

        private void Initialize()
        {
            slots.AddRange(slotStorage.GetComponentsInChildren<BookSlot>());
        }

        private void OpenTagedBoock<T>() where T : AbilitySO
        {
            var container = controller.GetContainer<T>();
            ResetSlots(container.Count);

            tempIndex = 0;
            container.Foreach(FillIn);
        }

        private void ResetSlots(int count)
        {
            SuppleSlots(count);
            TurnOffAll();
            TurnOnFor(count);
        }

        private void SuppleSlots(int count)
        {
            if (slots.Count < count)
            {
                for (int i = 0; i < count - slots.Count; i++)
                {
                    var clone = Instantiate(slots[i], slots[i].transform.parent);
                    slots.Add(clone);
                }
            }
        }

        private void TurnOffAll()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        private void TurnOnFor(int count)
        {
            for (int i = 0; i < count; i++)
            {
                slots[i].gameObject.SetActive(true);
            }
        }

        private void FillIn(AbilitySO data)
        {
            slots[tempIndex].Icon.sprite = data.colorIcon;
            tempIndex++;
        }
    }
}
