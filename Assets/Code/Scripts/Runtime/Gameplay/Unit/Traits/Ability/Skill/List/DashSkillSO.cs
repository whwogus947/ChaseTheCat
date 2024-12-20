using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Dash", menuName = "Cum2usGameDev/Ability/Skill/List/Dash")]
    public class DashSkillSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(DashSkillSO);

        protected override void PowerUp()
        {
            
        }
    }
}
