using UnityEngine;

namespace Com2usGameDev
{
    public abstract class AbilityViewGroup<T> : MonoBehaviour where T : AbilitySO
    {
        public abstract void AddAbility(T ability);
        public abstract void RemoveAbility(T ability);
    }
}