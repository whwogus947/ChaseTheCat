using UnityEngine;

namespace Com2usGameDev
{
    public abstract class TraitTypeSO : ScriptableObject
    {
        public abstract void AddAbility(IAbilities to);
    }
}