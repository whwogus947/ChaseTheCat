using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Ability Controller", menuName = "Cum2usGameDev/Ability/Controller")]
    public class AbilityController : ResettableSO
    {
        private Dictionary<string, AbilityContainer<AbilitySO>> container;

        public void AddAbility(AbilitySO ability)
        {
            if (!container.ContainsKey(ability.AbilityType))
                container[ability.AbilityType] = new();
            container[ability.AbilityType].Add(ability);
        }

        public bool HasAbility(AbilitySO ability)
            => container.ContainsKey(ability.AbilityType) && container[ability.AbilityType].Has(ability);

        public bool HasAbility(string abilityType, string abilityName)
            => container.ContainsKey(abilityType) && container[abilityType].Has(abilityName);

        public override void Initialize()
        {
            container = new();
            Debug.Log("Ability Controller Has Initialized");
        }
    }

    public class AbilityContainer<T> where T : AbilitySO
    {
        private readonly List<T> abilities;

        public AbilityContainer()
        {
            abilities = new();
        }

        public void Add(T ability) => abilities.Add(ability);

        public bool Has(T ability) => abilities.Contains(ability);

        public bool Has(string abilityName) => abilities.Exists(ability => ability.AbilityName == abilityName);
    }
}
