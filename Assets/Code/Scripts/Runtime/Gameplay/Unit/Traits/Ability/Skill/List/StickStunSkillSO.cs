using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Stick Stun", menuName = "Cum2usGameDev/Ability/Skill/List/StickStunSkill")]
    public class StickStunSkillSO : SkillAbility
    {
        public override string AbilityName => nameof(StickStunSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
