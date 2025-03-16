using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class AbilityHandler<T> where T : AbilitySO
    {
        protected AbilityController controller;
        protected readonly List<T> abilities = new();
        protected T currentAbility;
        protected AbilityContainer<T> Ability => controller.GetContainer<T>();
        protected AbilityViewGroup<T> viewGroup;

        protected abstract void AddInitialAbilities(List<T> initialItems);

        public AbilityHandler(IAbilityBundle<T> bundle)
        {
            controller = bundle.Controller;
            viewGroup = bundle.ViewGroup;
        }
    }
}
