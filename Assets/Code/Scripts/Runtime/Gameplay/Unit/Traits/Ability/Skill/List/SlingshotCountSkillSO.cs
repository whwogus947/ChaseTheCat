using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Slingshot Count Skill", menuName = "Cum2usGameDev/Ability/Skill/List/SlingshotCountSkill")]
    public class SlingshotCountSkillSO : SkillAbility
    {
        public override string AbilityName => nameof(SlingshotCountSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
