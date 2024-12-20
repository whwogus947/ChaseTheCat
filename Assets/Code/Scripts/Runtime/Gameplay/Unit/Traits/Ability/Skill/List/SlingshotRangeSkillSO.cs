using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Slingshot Range", menuName = "Cum2usGameDev/Ability/Skill/List/Slingshot Range")]
    public class SlingshotRangeSkillSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(SlingshotRangeSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
