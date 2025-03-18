using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [Serializable]
    public class FlotData
    {
        public string Name;
        public BookData Book;
    }

    public class SavableStageData
    {

    }

    [System.Serializable]
    public class BookData : ISaveable
    {
        public readonly Dictionary<Type, List<SavableProperty>> savedAbilities = new();

        public void Clear() => savedAbilities.Clear();

        public void ToSaveData(SavableProperty savableProperty)
        {
            if (savableProperty.type == null)
                return;

            if (!savedAbilities.ContainsKey(savableProperty.type))
                savedAbilities[savableProperty.type] = new();

            var abilityList = savedAbilities[savableProperty.type];
            SavableProperty saved = abilityList.Find(x => x.id == savableProperty.id);
            if (saved == null)
            {
                abilityList.Add(savableProperty);
            }
            else
            {
                abilityList.Remove(saved);
                abilityList.Add(savableProperty);
            }
        }
    }
}
