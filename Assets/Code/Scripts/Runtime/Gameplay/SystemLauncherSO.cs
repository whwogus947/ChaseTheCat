using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class SystemLauncherSO : ScriptableObject
    {
        [SerializeField] private InterfaceReference<IInitializeComponent>[] configs;

        public void Initiate()
        {
            foreach (var config in configs)
                config.Value.Initialize();
        }
    }

    public interface IInitializeComponent
    {
        void Initialize();
    }
}
