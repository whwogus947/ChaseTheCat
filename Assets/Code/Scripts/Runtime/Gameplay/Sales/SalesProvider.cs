using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    [RequireComponent(typeof(SalesViewer))]
    public class SalesProvider : ValidatedMonoBehaviour
    {
        [SerializeField] private InterfaceReference<ISalesItem>[] items;
        [SerializeField, Self] private SalesViewer viewer;
        [SerializeField] private Button openButton;

        void Awake()
        {
            openButton.onClick.AddListener(Provide);
        }

        void Start()
        {
        }

        public void Provide()
        {
            ISalesItem[] temp = new ISalesItem[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                temp[i] = items[i].Value;
            }
            viewer.Open(temp);
        }
    }
}
