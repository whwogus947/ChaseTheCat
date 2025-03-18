using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace Com2usGameDev
{
    public class SalesViewer : ValidatedMonoBehaviour
    {
        [SerializeField] private Transform storage;
        private readonly List<SalesItem> slots = new();

        void Awake()
        {
            slots.AddRange(GetComponentsInChildren<SalesItem>(true));
        }

        public void Open(ISalesItem[] items)
        {
            storage.gameObject.SetActive(true);
            
            ResetSlot(items);
            SetSlotsData(items);
        }

        private void ResetSlot(ISalesItem[] items)
        {
            int count = items.Length;
            int gap = count - slots.Count;
            if (gap <= 0)
                return;

            for (int i = 0; i < gap; i++)
            {
                var clone = Instantiate(slots[0], slots[0].transform.parent);
                slots.Add(clone);
            }
        }

        private void SetSlotsData(ISalesItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                slots[i].SetItem(item.Profile, item.SalesName, item.Price, items[i]);
            }
        }
    }
}
