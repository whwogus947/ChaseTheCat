using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Stamina", menuName = "Cum2usGameDev/Ability/Stat/List/Stamina")]
    public class StaminaStatSO : StatAbility
    {
        public override string AbilityName => nameof(StaminaStatSO);

        public override void OnAquire(AbilityController controller)
        {
            throw new System.NotImplementedException();
        }
    }
}
