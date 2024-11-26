using System;
using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class StateController : IStateAddible
    {
        public UnitBehaviour unitBehaviour;

        private readonly StateMachine[] machines;
        
        public StateController()
        {
            int length = Enum.GetValues(typeof(State)).Length;
            machines = new StateMachine[length];
            for (int i = 0; i < length; i++)
            {
                machines[i] = new(unitBehaviour);
            }
        }

        public void Initiate()
        {
            NodeTransition transition = new();
            StateNode.Builder<Nodes.Walk>.CreateType(State.Action).WithTransition(transition).Build(this);
        }

        public void AddState(IState state) => GetMachine(state.NodeState).Add(state);

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
