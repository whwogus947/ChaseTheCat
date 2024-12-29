using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Slingshot Count Skill", menuName = "Cum2usGameDev/Ability/Skill/List/SlingshotCountSkill")]
    public class SlingshotCountSkillSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(SlingshotCountSkillSO);
        public int Count => level;

        protected override void PowerUp()
        {
            
        }
    }
}
