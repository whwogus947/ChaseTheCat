using UnityEngine;

namespace Com2usGameDev
{
    public abstract class AbilitySO : ScriptableObject, IAbility
    {
        public abstract string AbilityName { get; }
    }
}
