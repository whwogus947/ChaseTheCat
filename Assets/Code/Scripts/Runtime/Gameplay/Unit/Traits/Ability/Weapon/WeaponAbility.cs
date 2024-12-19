using UnityEngine;

namespace Com2usGameDev
{
    public abstract class WeaponAbility : AbilitySO, IWeapon
    {
        public override string AbilityType => nameof(WeaponAbility);
    }
}
