using UnityEngine;

namespace Com2usGameDev
{
    public abstract class AbilitySO : ScriptableObject, IAbility
    {
        public abstract string AbilityType { get; }
        public abstract string AbilityName { get; }
        public GradeTypeSO grade;
        public Sprite colorIcon;
        public bool IsObtainable { get; set; } = true;

        public abstract void OnAquire();

        public virtual void OnDiscover()
        {
            IsObtainable = true;
        }
    }
}
