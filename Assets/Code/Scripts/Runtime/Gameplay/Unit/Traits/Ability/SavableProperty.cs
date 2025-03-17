using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Com2usGameDev
{
    [System.Serializable]
    public class SavableProperty
    {
        public Type type;
        public int id;

        public virtual AbilitySO AsAbility(AbilityController controller)
        {
            return null;
        }

        protected T AddAbilityTypeAs<T>(AbilityController controller) where T : AbilitySO
        {
            var target = controller.database.FindItem<T>(this);
            controller.AddAbility(target);
            return target;
        }
    }

    [System.Serializable]
    public class SavableSkillData : SavableProperty
    {
        public SavableSkillData(Type _type, int _id)
        {
            type = _type;
            id = _id;
        }

        public override AbilitySO AsAbility(AbilityController controller)
        {
            return AddAbilityTypeAs<SkillAbilitySO>(controller);
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

        public override AbilitySO AsAbility(AbilityController controller)
        {
            return AddAbilityTypeAs<StatAbilitySO>(controller);
        }
    }

    [System.Serializable]
    public class SavableNPCData : SavableProperty
    {
        public SavableNPCData(Type _type, int _id)
        {
            type = _type;
            id = _id;
        }

        public override AbilitySO AsAbility(AbilityController controller)
        {
            return AddAbilityTypeAs<NPCAbilitySO>(controller);
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

        public override AbilitySO AsAbility(AbilityController controller)
        {
            return AddAbilityTypeAs<WeaponAbilitySO>(controller);
        }
    }

    [System.Serializable]
    public class SavableMonsterData : SavableProperty
    {
        public SavableMonsterData(Type _type, int _id)
        {
            type = _type;
            id = _id;
        }

        public override AbilitySO AsAbility(AbilityController controller)
        {
            return AddAbilityTypeAs<MonsterAbilitySO>(controller);
        }
    }
}
