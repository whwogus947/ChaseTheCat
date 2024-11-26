using UnityEngine;

namespace Com2usGameDev.Dev
{
    public interface IState
    {
        public static IState Empty => null;
        State NodeState {get;}
        void OnEnter(UnitBehaviour unit);
        void OnUpdate(UnitBehaviour unit);
        void OnExit(UnitBehaviour unit);
        bool IsMovable(IState target);
        bool HasSatisfiedState(out IState state);
    }

    public interface ITransition
    {
        IState To { get; }
        ICondition Condition { get; }
    }

    public interface ICondition
    {
        bool Evaluate();
    }
}
