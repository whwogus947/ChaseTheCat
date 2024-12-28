using System;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class SpecialBehaviourSO<T> : SpecialSO where T : StateNode
    {
        public override Type BehaviourType => typeof(T);
    }
}
