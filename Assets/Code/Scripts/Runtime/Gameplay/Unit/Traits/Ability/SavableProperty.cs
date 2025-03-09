using UnityEngine;

namespace Com2usGameDev
{
    [System.Serializable]
    public class SavableProperty
    {
        public string typeName;
        public int id;
    }

    [System.Serializable]
    public class SavableSkillData : SavableProperty
    {
        public SavableSkillData(string _typeName, int _id)
        {
            typeName = _typeName;
            id = _id;
        }
    }

    [System.Serializable]
    public class SavableStatData : SavableProperty
    {
        public SavableStatData(string _typeName, int _id)
        {
            typeName = _typeName;
            id = _id;
        }
    }

    [System.Serializable]
    public class SavableWeaponData : SavableProperty
    {
        public int? count;

        public SavableWeaponData(string _typeName, int _id, int? _count)
        {
            typeName = _typeName;
            id = _id;
            count = _count;
        }
    }

    public interface ISavableProperty<T> where T : IAbility
    {
        void Save(T data);
        T Load();
    }
}
