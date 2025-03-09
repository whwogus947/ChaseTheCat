using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [System.Serializable]
    public class AbilityDatabase : IBind<BookData>
    {
        public List<DataBundle> bundles = new();
        public BookData book;

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

        public void Bind(BookData data)
        {
            book = data;
        }

        public void FromSavedData()
        {
            foreach (var dataByType in book.savedAbilities.Values)
            {
                for (int i = 0; i < dataByType.Count; i++)
                {
                    int id = dataByType[i].id;
                    DataBundle bundle = bundles.Find(x => x.typeName == dataByType[i].typeName);
                    bundle.abilities[id].FromSavedData(dataByType[i]);
                }
            }
        }

        public AbilitySO FromDB(string typeName, int id)
        {
            DataBundle bundle = bundles.Find(x => x.typeName == typeName);
            return bundle.abilities[id];
        }

        public void EnrollBook(AbilitySO ability)
        {
            ability.ToSaveData(book);
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
        public List<AbilitySO> abilities;

        public DataBundle(string _typeName)
        {
            typeName = _typeName;
            abilities = new();
        }

        public void Add(AbilitySO ability) => abilities.Add(ability);
    }
}
