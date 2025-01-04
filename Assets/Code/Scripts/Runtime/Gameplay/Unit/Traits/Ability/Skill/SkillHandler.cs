using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class SkillHandler : AbilityHandler<SkillAbilitySO>
    {
        public SkillHandler(IAbilityBundle<SkillAbilitySO> bundle) : base(bundle)
        {
            Ability.AddListener(AddSkill);
            AddInitialAbilities(bundle.Holder.initialItems);
        }

        public void UpdateAllSkills()
        {
            Ability?.Foreach(x => SkillUpdate(x));
        }

        private void SkillUpdate(SkillAbilitySO skill)
        {
            skill.CoolDown();
        }

        private void AddSkill(SkillAbilitySO skill)
        {
            viewGroup.AddAbility(skill);
        }

        protected override void AddInitialAbilities(List<SkillAbilitySO> initialItems)
        {
            initialItems.ForEach(x => Ability.Add(x));
        }
    }
}
