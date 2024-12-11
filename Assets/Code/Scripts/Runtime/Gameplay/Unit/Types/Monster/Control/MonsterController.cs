using UnityEngine;

namespace Com2usGameDev
{
    [RequireComponent(typeof(MonsterBehaviour))]
    public class MonsterController : MonoBehaviour
    {
        private StateController controller;
        private MonsterBehaviour behaviour;

        private void Awake()
        {
            behaviour = GetComponent<MonsterBehaviour>();
        }

        void Start()
        {
            controller = new StateController(behaviour);
            BindBehaviourToController();
        }

        void Update()
        {
            controller.Run();
        }

        public void BindBehaviourToController()
        {
            var empty = StateNode.Creator<Nodes.Empty>.CreateType(State.Basic).InProgress();
            var idle = StateNode.Creator<Nodes.MonIdle>.CreateType(State.Basic).InProgress();
            var walk = StateNode.Creator<Nodes.MonWalk>.CreateType(State.Basic).InProgress();
            // var run = StateNode.Creator<Nodes.MonRun>.CreateType(State.Basic).InProgress();
            // var attackNormal = StateNode.Creator<Nodes.MonAttack>.CreateType(State.Basic).InProgress();
            // var dead = StateNode.Creator<Nodes.MonDead>.CreateType(State.Basic).InProgress();
            
            NodeTransition toIdle = new(idle, new(() => !behaviour.IsPlayerNearby() || !behaviour.IsChasable()));
            NodeTransition toWalk = new(walk, new(() => behaviour.IsPlayerNearby() && behaviour.IsChasable()));

            StateNode.Creator<Nodes.Empty>.Using(empty).WithTransition(toIdle).WithTransition(toWalk).Accomplish(controller);
            StateNode.Creator<Nodes.MonIdle>.Using(idle).WithTransition(toWalk).WithAnimation("mob_idle").Accomplish(controller);
            StateNode.Creator<Nodes.MonWalk>.Using(walk).WithTransition(toIdle).WithAnimation("mob_walk").Accomplish(controller);            
        }
    }
}
