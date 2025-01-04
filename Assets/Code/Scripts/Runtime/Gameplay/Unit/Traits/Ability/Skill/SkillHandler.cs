using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class SkillHandler : AbilityHandler<SkillAbilitySO>
    {
        public SkillHandler(IAbilityBundle<SkillAbilitySO> bundle) : base(bundle)
        {

        }

        private void SkillUpdate(SkillAbilitySO skill)
        {
            skill.CoolDown();
        }

        private void AddSkill(SkillAbilitySO skill)
        {
            
        }

        protected override void AddInitialAbilities(List<SkillAbilitySO> initialItems)
        {
            // skillViewGroup.AddAbility(skill);
        }
    }
}
