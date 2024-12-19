using UnityEngine;

namespace Com2usGameDev
{
    public abstract class StatAbility : AbilitySO, ISkill
    {
        public override string AbilityType => nameof(StatAbility);
    }
}
