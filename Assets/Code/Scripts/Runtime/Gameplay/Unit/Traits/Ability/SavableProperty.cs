using System;
using UnityEngine;

namespace Com2usGameDev
{
    [System.Serializable]
    public class SavableProperty
    {
        public Type type;
        public int id;
    }

    [System.Serializable]
    public class SavableSkillData : SavableProperty
    {
        public SavableSkillData(Type _type, int _id)
        {
            type = _type;
            id = _id;
        }
    }

    [System.Serializable]
    public class SavableStatData : SavableProperty
    {
        public SavableStatData(Type _type, int _id)
        {
            type = _type;
            id = _id;
        }
    }

    [System.Serializable]
    public class SavableWeaponData : SavableProperty
    {
        public int? count;

        public SavableWeaponData(Type _type, int _id, int? _count)
        {
            type = _type;
            id = _id;
            count = _count;
        }
    }
}
