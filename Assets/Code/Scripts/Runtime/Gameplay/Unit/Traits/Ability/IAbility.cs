using UnityEngine;

namespace Com2usGameDev
{
    public interface IAbility
    {

    }

    public interface IAbilityBundle<T> where T : AbilitySO
    {
        AbilityController Controller { get; }
        AbilityViewGroup<T> ViewGroup { get; }
        AbilityHolder<T> Holder { get; }
    }

    public interface ISkill : IAbility
    {

    }

    public interface IStat : IAbility
    {

    }

    public interface IWeapon : IAbility
    {

    }
}
