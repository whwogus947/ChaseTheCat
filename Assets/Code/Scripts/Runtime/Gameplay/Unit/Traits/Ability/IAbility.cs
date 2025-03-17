using UnityEngine;

namespace Com2usGameDev
{
    public interface IAbilityBundle<T> where T : AbilitySO
    {
        AbilityController Controller { get; }
        AbilityViewGroup<T> ViewGroup { get; }
        AbilityHolder<T> Holder { get; }
    }
}
