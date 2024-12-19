using UnityEngine;

namespace Com2usGameDev
{
    public abstract class ResettableSO : ScriptableObject
    {
        //this SO must be located in Resources folder
        public abstract void Initialize();
    }
}
