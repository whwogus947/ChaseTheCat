using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Poison Damage Skill", menuName = "Cum2usGameDev/Ability/Skill/List/PoisonDamageSkill")]
    public class PoisonDamageSkillSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(PoisonDamageSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
