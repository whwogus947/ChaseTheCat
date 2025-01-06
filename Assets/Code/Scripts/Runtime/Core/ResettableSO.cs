using UnityEngine;

namespace Com2usGameDev
{
    public abstract class ResettableSO : ScriptableObject, IInitializeComponent
    {
        public abstract void Initialize();
    }
}
