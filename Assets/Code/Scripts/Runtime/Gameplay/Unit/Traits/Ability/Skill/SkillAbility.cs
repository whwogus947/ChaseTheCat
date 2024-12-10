using UnityEngine;

namespace Com2usGameDev
{
    public class SkillAbility : AbilitySO, ISkill
    {
        public override string AbilityName { get => nameof(SkillAbility); }
    }
}
