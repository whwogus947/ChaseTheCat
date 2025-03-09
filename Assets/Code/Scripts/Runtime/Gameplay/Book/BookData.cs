using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [System.Serializable]
    public class BookData : ISaveable
    {
        public Dictionary<string, List<SavableProperty>> savedAbilities = new();

        // public void EnrollBook(AbilitySO ability)
        // {
        //     var type = ability.AbilityType;
        //     var id = ability.ID;
        //     if (!abilities.ContainsKey(type))
        //     {
        //         abilities[type] = new() { id };
        //         return;
        //     }
        //     if (!abilities[type].Contains(id))
        //         abilities[type].Add(id);
        // }

        public void EnrollBook(SavableProperty savableProperty)
        {
            if (!savedAbilities.ContainsKey(savableProperty.typeName))
            {
                savedAbilities[savableProperty.typeName] = new();
            }
            if (!savedAbilities[savableProperty.typeName].Contains(savableProperty))
                savedAbilities[savableProperty.typeName].Add(savableProperty);
        }
    }
}
