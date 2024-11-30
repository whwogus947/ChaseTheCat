using System;
using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class StateController : IStateAddible
    {
        private readonly StateMachine[] machines;
        
        public StateController(UnitBehaviour unitBehaviour)
        {
            int length = Enum.GetValues(typeof(State)).Length;
            machines = new StateMachine[length];
            for (int i = 0; i < length; i++)
            {
                machines[i] = new(unitBehaviour);
            }
        }
        
        public void AddState(IState state) => GetMachine(state.NodeState).Add(state);

        public bool IsState(State nodeState, Type state)
        {
            return GetMachine(nodeState).IsState(state);
        }

        private StateMachine GetMachine(State nodeState)
        {
            var index = (int)nodeState;
            return machines[index];
        }

        public void Run()
        {
            for (int i = 0; i < machines.Length; i++)
            {
                machines[i].Update();
            }
        }
    }
}
