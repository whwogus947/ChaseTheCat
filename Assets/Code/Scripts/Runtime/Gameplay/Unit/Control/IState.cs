using System;

namespace Com2usGameDev
{
    public interface IState
    {
        public static IState Empty => null;
        State NodeState {get;}
        void OnInitialize(UnitBehaviour unit);
        void OnEnter(UnitBehaviour unit);
        void OnUpdate(UnitBehaviour unit);
        void OnExit(UnitBehaviour unit);
        bool HasSatisfiedState(out IState state);
        void RemoveTransition(Type state);
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

    public interface IStateAddible
    {
        void AddState(IState state);
    }
}
