using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [System.Serializable]
    public class AbilityDatabase
    {
        public List<DataBundle> bundles = new();

        public void Add(AbilitySO ability)
        {
            var bundle = bundles.Find(x => x.typeName == ability.AbilityType);
            if (bundle == null)
            {
                bundle = new DataBundle(ability.AbilityType);
                bundles.Add(bundle);
            }
            if (ability.ID == -1)
                bundle.Add(ability);
        }

        public void RegenerateID()
        {
            foreach (var bundle in bundles)
            {
                for (int i = 0; i < bundle.abilities.Count; i++)
                {
                    bundle.abilities[i].ID = i;
                }
            }
        }
    }

    [System.Serializable]
    public class DataBundle
    {
        [ReadOnly] public string typeName;
        public List<AbilitySO> abilities;

        public DataBundle(string _typeName)
        {
            typeName = _typeName;
            abilities = new();
        }

        public void Add(AbilitySO ability) => abilities.Add(ability);
    }
}
