using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Screaming Echo", menuName = "Cum2usGameDev/Ability/Skill/List/ScreamingEcho")]
    public class ScreamingEchoSkillSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(ScreamingEchoSkillSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
