using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class UnitStateMachine : MonoBehaviour
    {
        public bool IsTransmittable { get; set; }

        protected Dictionary<Type, IState> states;
        protected Type currentStateType;
        protected UnitBehaviour behaviour;

        private void Awake()
        {
            Initialize();
        }

        public abstract void OnUpdate();

        protected abstract void Initialize();

        public void ChangeState(Type to)
        {
            if (currentStateType == to || !states[to].HasEnoughEP(behaviour))
                return;

            states[currentStateType].OnExit(behaviour);
            currentStateType = to;
            states[currentStateType].OnEnter(behaviour);
        }

        protected bool IsCurrentStateType(Type stateType)
        {
            return currentStateType == stateType;
        }
    }
}
