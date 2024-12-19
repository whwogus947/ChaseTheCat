using UnityEngine;

namespace Com2usGameDev
{
    public abstract class AbilitySO : ScriptableObject, IAbility
    {
        public abstract string AbilityType { get; }
        public abstract string AbilityName { get; }
        public GradeTypeSO grade;

        public abstract void OnAquire(AbilityController controller);
    }
}
