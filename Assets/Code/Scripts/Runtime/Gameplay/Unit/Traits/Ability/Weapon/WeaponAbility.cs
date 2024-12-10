using UnityEngine;

namespace Com2usGameDev
{
    public class WeaponAbility : AbilitySO, IWeapon
    {
        public override string AbilityName => nameof(WeaponAbility);
    }
}
