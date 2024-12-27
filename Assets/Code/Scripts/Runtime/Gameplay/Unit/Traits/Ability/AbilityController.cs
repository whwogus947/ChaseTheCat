using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Ability Controller", menuName = "Cum2usGameDev/Ability/Controller")]
    public class AbilityController : ResettableSO
    {
        private Dictionary<string, IAbilityContainer> containers = new();

        public void AddAbility<T>(T ability) where T : AbilitySO
        {
            if (!containers.ContainsKey(ability.AbilityType))
                containers[ability.AbilityType] = new AbilityContainer<AbilitySO>();
            containers[ability.AbilityType].Add(ability);
        }

        public bool HasAbility<T>(T ability) where T : AbilitySO
            => containers.ContainsKey(ability.AbilityType) && containers[ability.AbilityType].Has(ability);

        public bool HasAbility(string abilityType, string abilityName)
            => containers.ContainsKey(abilityType) && containers[abilityType].Has(abilityName);

        public T GetAbility<T>(string abilityType, string abilityName) where T : AbilitySO
        {
            if (!containers.ContainsKey(abilityType))
                return null;
            return containers[abilityType].Find(abilityName) as T;
        }

        public override void Initialize()
        {
            containers = new();
            containers.Clear();
            Debug.Log("Ability Controller Has Initialized");
        }

        public AbilityContainer<T> GetContainer<T>(string abilityType) where T : AbilitySO
        {
            if (!containers.ContainsKey(abilityType))
                containers[abilityType] = new AbilityContainer<T>();
            return containers[abilityType] as AbilityContainer<T>;
        }
    }

    public interface IAbilityContainer
    {
        void Add(AbilitySO ability);
        bool Has(AbilitySO ability);
        bool Has(string abilityName);
        AbilitySO Find(string abilityName);
    }

    public class AbilityContainer<T> : IAbilityContainer where T : AbilitySO
    {
        private readonly List<T> abilities;
        private UnityAction<T> onAddContainer;

        public AbilityContainer()
        {
            abilities = new();
            onAddContainer = delegate { };
        }

        public void Add(AbilitySO ability)
        {
            T casted = ability as T;
            if (!abilities.Contains(casted))
            {
                onAddContainer(casted);
                abilities.Add(casted);
                ability.OnDiscover();
            }
            else
            {
                ability.OnAquire();
            }
        }

        public void AddListener(UnityAction<T> action) => onAddContainer += action;

        public bool Has(AbilitySO ability) => abilities.Contains(ability as T);

        public void Remove(AbilitySO ability) => abilities.Remove(ability as T);

        public bool Has(string abilityName) => abilities.Exists(ability => ability.AbilityName == abilityName);

        public AbilitySO Find(string abilityName) => abilities.Find(ability => ability.AbilityName == abilityName);

        public void Foreach(UnityAction<T> action)
        {
            foreach (var ability in abilities)
                action(ability);
        }
    }
}
