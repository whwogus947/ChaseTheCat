using UnityEngine;

namespace Com2usGameDev
{
    public abstract class AbilityViewGroup<T> : MonoBehaviour where T : AbilitySO
    {
        public abstract void AddAbility(T skill);
    }
}