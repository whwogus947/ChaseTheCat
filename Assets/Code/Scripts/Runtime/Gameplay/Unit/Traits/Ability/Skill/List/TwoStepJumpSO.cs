using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Two Step Jump", menuName = "Cum2usGameDev/Ability/Skill/List/TwoStepJump")]
    public class TwoStepJumpSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(TwoStepJumpSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
