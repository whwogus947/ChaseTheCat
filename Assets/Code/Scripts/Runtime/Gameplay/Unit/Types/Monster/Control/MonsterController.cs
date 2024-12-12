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
            var attackNormal = StateNode.Creator<Nodes.MonAttack>.CreateType(State.Basic).InProgress();
            // var dead = StateNode.Creator<Nodes.MonDead>.CreateType(State.Basic).InProgress();
            
            NodeTransition toEmpty = new(empty, new(() => behaviour.timer.IsFinished));
            NodeTransition toIdle = new(idle, new(() => behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && !behaviour.IsPlayerNearby() || !behaviour.IsChasable()));
            NodeTransition toWalk = new(walk, new(() => behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && behaviour.IsPlayerNearby() && behaviour.IsChasable()));
            NodeTransition toAttackNormal = new(attackNormal, new(() => behaviour.IsPlayerBeside() && behaviour.IsChasable()));

            StateNode.Creator<Nodes.Empty>.Using(empty).WithTransition(toIdle).WithTransition(toWalk).WithTransition(toAttackNormal).Accomplish(controller);
            StateNode.Creator<Nodes.MonIdle>.Using(idle).WithTransition(toWalk).WithTransition(toAttackNormal).WithAnimation("mob_idle").Accomplish(controller);
            StateNode.Creator<Nodes.MonWalk>.Using(walk).WithTransition(toIdle).WithTransition(toAttackNormal).WithAnimation("mob_walk").Accomplish(controller);            
            StateNode.Creator<Nodes.MonAttack>.Using(attackNormal).WithTransition(toIdle).WithTransition(toWalk).WithTransition(toEmpty).WithAnimation("mob_attack").Accomplish(controller);            
        }
    }
}
