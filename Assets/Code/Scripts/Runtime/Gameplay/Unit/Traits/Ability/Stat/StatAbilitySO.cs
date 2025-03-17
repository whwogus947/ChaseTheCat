using System;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class StatAbilitySO : AbilitySO
    {
        public override string AbilityTypeName => nameof(StatAbilitySO);
        public override Type AbilityType => typeof(StatAbilitySO);

        public override void ToSaveData(BookData book)
        {
            var data = new SavableSkillData(AbilityType, ID);
            book.ToSaveData(data);
        }

        public override void ConvertDataToInstance(SavableProperty data)
        {
            
        }
    }
}
