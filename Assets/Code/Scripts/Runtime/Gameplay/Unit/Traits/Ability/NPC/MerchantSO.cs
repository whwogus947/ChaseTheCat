using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Merchant", menuName = "Cum2usGameDev/Ability/NPC/List/Merchant")]
    public class MerchantSO : NPCAbilitySO
    {
        [Header("Data")]
        public string NPCName;
        public override string AbilityName => NPCName;

        
    }
}
