using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Rage", menuName = "Cum2usGameDev/Ability/Skill/List/Rage")]
    public class RageSkillSO : SkillAbility
    {
        public override string AbilityName => nameof(RageSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
