using System;

namespace Com2usGameDev.Dev
{
    public class NodeTransition : ITransition
    {
        public IState To => target;
        public ICondition Condition => predicate;

        public FuncPredicate predicate;
        public IState target;

        public NodeTransition(IState target, FuncPredicate predicate)
        {
            this.target = target;
            this.predicate = predicate;
        }
    }

    public class FuncPredicate : ICondition
    {
        private readonly Func<bool> predication;

        public FuncPredicate(Func<bool> predication)
        {
            this.predication = predication;
        }

        public bool Evaluate()
        {
            return predication.Invoke();
        }
    }
}
