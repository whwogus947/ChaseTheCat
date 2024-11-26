using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class StateMachine
    {
        private Type currentState;
        private readonly Dictionary<Type, IState> states;
        private UnitBehaviour behaviour;
        
        public StateMachine(UnitBehaviour behaviour)
        {
            states = new();
            this.behaviour = behaviour;
        }

        public void Update()
        {
            if (states.Count == 0)
                return;

            states[currentState].OnUpdate(behaviour);
        }

        public bool TryChangeState(Type target)
        {
            if (target == currentState)
                return false;

            if (states[currentState].IsMovable(states[target]))
            {
                states[currentState].OnExit(behaviour);
                currentState = target;
                states[currentState].OnEnter(behaviour);
            }
            return false;
        }
    }
}