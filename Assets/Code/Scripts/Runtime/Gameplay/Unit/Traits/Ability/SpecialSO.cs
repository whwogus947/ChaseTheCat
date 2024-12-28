using System;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class SpecialSO : ScriptableObject
    {
        public abstract Type BehaviourType { get; }
    }
}