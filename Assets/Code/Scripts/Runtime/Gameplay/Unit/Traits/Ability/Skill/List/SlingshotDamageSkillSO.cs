using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Slingshot Damage Skill", menuName = "Cum2usGameDev/Ability/Skill/List/SlingshotDamageSkill")]
    public class SlingshotDamageSkillSO : SkillAbility
    {
        public override string AbilityName => nameof(SlingshotDamageSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
