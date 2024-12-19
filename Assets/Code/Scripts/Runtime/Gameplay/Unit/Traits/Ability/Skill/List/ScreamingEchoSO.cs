using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Screaming Echo", menuName = "Cum2usGameDev/Ability/Skill/List/ScreamingEcho")]
    public class ScreamingEchoSO : SkillAbility
    {
        public override string AbilityName => nameof(ScreamingEchoSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
