using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class NodeTransition : ITransition
    {
        public IState To => throw new System.NotImplementedException();

        public ICondition Condition => throw new System.NotImplementedException();
    }
}
