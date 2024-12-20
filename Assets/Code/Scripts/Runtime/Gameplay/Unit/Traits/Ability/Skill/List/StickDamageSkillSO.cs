using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Stick Damage Reinforce", menuName = "Cum2usGameDev/Ability/Skill/List/StickDamageReinforce")]
    public class StickDamageSkillSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(StickDamageSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
