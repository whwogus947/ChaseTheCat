using System;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class MonsterAbilitySO : AbilitySO
    {
        public override string AbilityTypeName => nameof(MonsterAbilitySO);
        public override Type AbilityType => typeof(MonsterAbilitySO);
    }
}
