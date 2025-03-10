using System;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class AbilitySO : ScriptableObject, IAbility
    {
        [ReadOnly] public int ID = -1;
        public abstract string AbilityTypeName { get; }
        public abstract string AbilityName { get; }
        public abstract Type AbilityType { get; }
        public GradeTypeSO grade;
        public Sprite colorIcon;
        public bool IsObtainable { get; set; } = true;

        public abstract void OnAquire();

        public virtual void OnDiscover()
        {
            IsObtainable = true;
        }

        public abstract void ToSaveData(BookData book);

        public abstract void ConvertDataToInstance(SavableProperty data);
    }
}
