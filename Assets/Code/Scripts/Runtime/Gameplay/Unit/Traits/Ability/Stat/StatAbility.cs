using System;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class StatAbility : AbilitySO, ISkill
    {
        public override string AbilityTypeName => nameof(StatAbility);
        public override Type AbilityType => typeof(StatAbility);

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
