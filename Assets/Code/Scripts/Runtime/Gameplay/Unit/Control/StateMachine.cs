using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Com2usGameDev
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
            currentState = typeof(Nodes.Common.Empty);
        }

        public void Add(IState state)
        {
            states[state.GetType()] = state;
            state.OnInitialize(behaviour);
        }

        public void Update()
        {
            if (states.Count == 0)
                return;

            CheckNextTransition();
            UpdateCurrentState();
        }

        public bool IsState(Type state)
        {
            return currentState == state;
        }

        private void CheckNextTransition()
        {
            if (states[currentState].HasSatisfiedState(out IState state))
            {
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
            if (!IsValidate(target))
                return;

            states[currentState].OnExit(behaviour);
            currentState = target;
            states[currentState].OnEnter(behaviour);
        }

        private bool IsValidate(Type target)
        {
            if (target == currentState)
            {
                return false;
            }

            if (!states.ContainsKey(target))
            {
                UnityEngine.Debug.Log("Delete" + target);
                
                states[currentState].RemoveTransition(target);
                ChangeState(currentState);
                return false;
            }

            return true;
        }
    }
}