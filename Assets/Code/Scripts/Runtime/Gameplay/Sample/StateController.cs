using System;
using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class StateController
    {
        public UnitBehaviour unitBehaviour;
        public StateMachine[] machines;
        
        public StateController()
        {
            int length = Enum.GetValues(typeof(State)).Length;
            machines = new StateMachine[length];
            for (int i = 0; i < length; i++)
            {
                machines[i] = new(unitBehaviour);
            }
        }

        public StateMachine GetMachine(State nodeState)
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
