using UnityEngine;

namespace Com2usGameDev
{
    public abstract class StatAbility : AbilitySO, ISkill
    {
        public override string AbilityType => nameof(StatAbility);

        public override void ToSaveData(BookData book)
        {
            var data = new SavableSkillData(AbilityType, ID);
            book.EnrollBook(data);
        }

        public override void FromSavedData(SavableProperty data)
        {
            
        }
    }
}
