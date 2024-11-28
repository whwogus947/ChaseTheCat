using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class StateMachine
    {
        private Type currentState;
        private readonly Dictionary<Type, IState> states;
        private readonly UnitBehaviour behaviour;
        
        public StateMachine(UnitBehaviour behaviour)
        {
            states = new();
            this.behaviour = behaviour;
            currentState = typeof(Nodes.Idle);
        }

        public void Add(IState state)
        {
            states[state.GetType()] = state;
        }

        public void Update()
        {
            if (states.Count == 0)
                return;

            CheckNextTransition();
            UpdateCurrentState();
        }

        private void CheckNextTransition()
        {
            Debug.Log("check");
            if (states[currentState].HasSatisfiedState(out IState state))
            {
                Debug.Log(state.GetType());
                ChangeState(state.GetType());
                return;
            }
        }

        private void UpdateCurrentState()
        {
            states[currentState].OnUpdate(behaviour);
        }

        private void ChangeState(Type target)
        {
            if (target == currentState)
                return;

            states[currentState].OnExit(behaviour);
            currentState = target;
            states[currentState].OnEnter(behaviour);
        }
    }
}