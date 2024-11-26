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

            NodeTransition transition = new();

            StateNode test = StateNode.Builder<Nodes.Walk>.Create(State.Action).WithTransition(transition).Build();
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
            if (states[currentState].HasSatisfiedState(out IState state))
            {
                ChangeState(state.GetType());
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