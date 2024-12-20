using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Static Flight", menuName = "Cum2usGameDev/Ability/Skill/List/StaticFlight")]
    public class StaticFlightSO : SkillAbilitySO
    {
        public override string AbilityName => nameof(StaticFlightSO);

        protected override void PowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
