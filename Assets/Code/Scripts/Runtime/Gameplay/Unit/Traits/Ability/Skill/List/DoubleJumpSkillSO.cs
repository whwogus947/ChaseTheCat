using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Double Jump", menuName = "Cum2usGameDev/Ability/Skill/List/DoubleJump")]
    public class DoubleJumpSkillSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(DoubleJumpSkillSO);

        protected override void PowerUp()
        {
            
        }
    }
}
