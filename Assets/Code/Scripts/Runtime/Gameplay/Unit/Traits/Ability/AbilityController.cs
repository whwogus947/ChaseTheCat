using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class AbilityController : MonoBehaviour
    {
        private readonly Dictionary<string, AbilityContainer<AbilitySO>> container = new();

        public void AddAbility(AbilitySO ability)
        {
            if (!container.ContainsKey(ability.AbilityName))
                container[ability.AbilityName] = new();
            container[ability.AbilityName].Add(ability);
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
    }
}
