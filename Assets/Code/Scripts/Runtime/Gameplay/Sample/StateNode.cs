using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev.Dev
{
    public enum State
    {
        None,
        Movement,
        Action,
    }

    public abstract class StateNode : IState
    {
        public State NodeState {get; private set;}
        
        private readonly List<ITransition> transitions;

        public StateNode()
        {
            transitions = new();
        }

        public void SetNode(State state)
        {
            this.NodeState = state;
        }

        public abstract void OnEnter(UnitBehaviour unit);

        public abstract void OnExit(UnitBehaviour unit);

        public abstract void OnUpdate(UnitBehaviour unit);

        protected void AddTransition(ITransition transition)
        {
            transitions.Add(transition);
        }

        public bool IsMovable(IState target)
        {
            foreach (var transition in transitions)
            {
                if (transition.To == target && transition.Condition.Evaluate())
                    return true;
            }
            return false;
        }

        public bool HasSatisfiedState(out IState state)
        {
            state = IState.Empty;
            foreach (var transition in transitions)
            {
                if (transition.Condition.Evaluate())
                {
                    state = transition.To;
                    return true;
                }
            }
            return false;
        }


        public class Builder<T> where T : StateNode, new()
        {
            private T node;

            public static Builder<T> CreateType(State state)
            {
                var builder = new Builder<T>
                {
                    node = new()
                };
                builder.node.SetNode(state);
                return builder;
            }

            public Builder<T> WithTransition(ITransition transition)
            {
                node.AddTransition(transition);
                return this;
            }

            public Builder<T> WithAnimation(string animName)
            {
                return this;
            }

            public T Build() => node;

            public void Build(IStateAddible addible)
            {
                addible.AddState(node);
            }
        }
    }

    public class Nodes
    {
        public class Walk : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnExit(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
