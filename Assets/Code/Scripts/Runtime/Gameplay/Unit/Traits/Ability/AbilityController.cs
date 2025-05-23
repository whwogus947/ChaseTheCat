using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Ability Controller", menuName = "Cum2usGameDev/Ability/Controller")]
    public class AbilityController : ResettableSO, IBind<BookData>
    {
        public AbilityDatabase database;

        private Dictionary<string, IAbilityContainer> containers = new();
        private BookData book;

        public void AddAbility<T>(T ability) where T : AbilitySO
        {
            if (!containers.ContainsKey(ability.AbilityTypeName))
                containers[ability.AbilityTypeName] = new AbilityContainer<T>();
            containers[ability.AbilityTypeName].Add(ability);
            ability.ToSaveData(book);
        }

        public void RemoveAbility<T>(T ability) where T : AbilitySO
        {
            if (containers.ContainsKey(ability.AbilityTypeName))
                containers[ability.AbilityTypeName].Remove(ability);
        }

        public bool HasAbility<T>(T ability) where T : AbilitySO
            => containers.ContainsKey(ability.AbilityTypeName) && containers[ability.AbilityTypeName].Has(ability);

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

        public AbilityContainer<T> GetContainer<T>() where T : AbilitySO
        {
            string abilityType = typeof(T).Name;
            if (!containers.ContainsKey(abilityType))
                containers[abilityType] = new AbilityContainer<T>();
            return containers[abilityType] as AbilityContainer<T>;
        }

        [ContextMenu("Regenerate ID")]
        private void RegenerateDatabaseAll()
        {
            database.RegenerateID();
        }

        public void Bind(BookData data)
        {
            book = data;
        }

        public void FromSavedData(Dictionary<Type, List<SavableProperty>> clone)
        {
            Clear();
            book.savedAbilities.Clear();
            foreach (var (type, savableData) in clone)
            {
                for (int i = 0; i < savableData.Count; i++)
                {
                    var target = savableData[i].AsAbility(this);
                    target.ConvertDataToInstance(savableData[i]);
                }
            }
        }

        private void Clear()
        {
            foreach (var (typeName, container) in containers)
            {
                container.Clear();
            }
        }
    }

    public interface IAbilityContainer
    {
        void Add(AbilitySO ability);
        void Remove(AbilitySO ability);
        bool Has(AbilitySO ability);
        bool Has(string abilityName);
        AbilitySO Find(string abilityName);
        void Clear();
        int Count { get; }
    }

    public class AbilityContainer<T> : IAbilityContainer where T : AbilitySO
    {
        private readonly List<T> abilities;
        private UnityAction<T> onAddContainer;
        private UnityAction<T> onRemoveContainer;

        public int Count => abilities.Count;

        public AbilityContainer()
        {
            abilities = new();
            onAddContainer = delegate { };
            onRemoveContainer = delegate { };
        }

        public void Add(AbilitySO ability)
        {
            if (ability is T casted && !abilities.Contains(casted))
            {
                Debug.Log("<color=red>new!!</color> " + ability.AbilityName);
                // if (casted is WeaponAbilitySO)
                //     Debug.Log("<color=yellow>new!!</color>" + typeof(T));
                onAddContainer(casted);
                abilities.Add(casted);
                ability.OnDiscover();
            }
            else
            {
                Debug.Log("<color=red>already!!</color> " + ability.AbilityName);
                ability.OnAquire();
            }
        }

        public void AddAquireListener(UnityAction<T> action) => onAddContainer += action;

        public void AddRemovalListener(UnityAction<T> action) => onRemoveContainer += action;

        public bool Has(AbilitySO ability) => abilities.Contains(ability as T);

        public void Remove(AbilitySO ability)
        {
            if (ability is T casted && abilities.Contains(casted))
            {
                Debug.Log("Try remove " + ability);
                onRemoveContainer(casted);
                abilities.Remove(casted);
            }
        }

        public void Clear() => Foreach(Remove);

        public bool Has(string abilityName) => abilities.Exists(ability => ability.AbilityName == abilityName);

        public AbilitySO Find(string abilityName) => abilities.Find(ability => ability.AbilityName == abilityName);

        public void Foreach(UnityAction<T> action)
        {
            for (int i = abilities.Count - 1; i >= 0; i--)
                action(abilities[i]);
        }
    }
}
