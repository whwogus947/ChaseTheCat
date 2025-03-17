using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Stamina", menuName = "Cum2usGameDev/Ability/Stat/List/Stamina")]
    public class StaminaStatSO : StatAbilitySO
    {
        public override string AbilityName => nameof(StaminaStatSO);

        public override void OnAquire()
        {
            throw new System.NotImplementedException();
        }
    }
}
