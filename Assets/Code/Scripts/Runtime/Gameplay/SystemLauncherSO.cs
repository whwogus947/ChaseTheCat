using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class SystemLauncherSO : ScriptableObject
    {
        public List<Manager> managers;
        public List<ResettableSO> resettables;
        
        public void Initiate()
        {
            foreach (var manager in managers)
            {
                var clone = Instantiate(manager);
                DontDestroyOnLoad(clone);
            }

            foreach (var resettable in resettables)
            {
                resettable.Initialize();
            }
        }
    }
}
