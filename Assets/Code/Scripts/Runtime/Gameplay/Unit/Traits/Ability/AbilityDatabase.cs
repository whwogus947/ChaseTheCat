using System;
using System.Collections;
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
            var bundle = bundles.Find(x => x.typeName == ability.AbilityType.ToString());
            if (bundle == null)
            {
                bundle = new DataBundle(ability.AbilityType.ToString(), ability.AbilityType);
                bundles.Add(bundle);
                Debug.Log(ability.AbilityTypeName);
                Debug.Log(ability.AbilityType);
            }
            if (ability.ID == -1)
                bundle.Add(ability);
            else
            {
                if (!bundle.abilities.Contains(ability))
                    bundle.Add(ability);
            }
        }
        
        public void RegenerateID()
        {
            foreach (var bundle in bundles)
            {
                for (int i = 0; i < bundle.abilities.Count; i++)
                {
                    bundle.abilities[i].ID = i;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(bundle.abilities[i]);
                    UnityEditor.AssetDatabase.SaveAssetIfDirty(bundle.abilities[i]);
#endif
                }
            }
        }
    }

    [System.Serializable]
    public class DataBundle
    {
        [ReadOnly] public string typeName;
        public Type type;
        public List<AbilitySO> abilities;

        public DataBundle(string _typeName, Type _type)
        {
            typeName = _typeName;
            type = _type;
            abilities = new();
        }

        public void Add(AbilitySO ability)
        {
            abilities.Add(ability);
        }
    }
}
