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
            var empty = StateNode.Creator<Nodes.Common.Empty>.CreateType(State.Basic).InProgress();
            var idle = StateNode.Creator<Nodes.Enemy.Idle>.CreateType(State.Basic).InProgress();
            var chasing = StateNode.Creator<Nodes.Enemy.Walk>.CreateType(State.Basic).InProgress();
            var roam = StateNode.Creator<Nodes.Enemy.Walk>.CreateType(State.Basic).InProgress();
            // var run = StateNode.Creator<Nodes.MonRun>.CreateType(State.Basic).InProgress();
            var attackNormal = StateNode.Creator<Nodes.Enemy.Attack>.CreateType(State.Basic).InProgress();
            // var dead = StateNode.Creator<Nodes.MonDead>.CreateType(State.Basic).InProgress();
            
            NodeTransition toEmpty = new(empty, new(() => behaviour.timer.IsFinished));
            // NodeTransition toIdle = new(idle, new(() => !roam.IsRoaming() && behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && !behaviour.IsPlayerNearby() || (!behaviour.IsChasable() && !roam.IsRoaming())));
            NodeTransition toIdle = new(idle, new(() => !roam.IsRoaming() && behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && !behaviour.IsPlayerNearby() || !behaviour.IsChasable()));
            NodeTransition toChasing = new(chasing, new(() => behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && behaviour.IsPlayerNearby() && behaviour.IsChasable()));
            NodeTransition toRoam = new(roam, new(() => idle.IsWandering() && !behaviour.IsPlayerBeside() && !behaviour.IsPlayerNearby() && behaviour.IsChasable()));
            NodeTransition toAttackNormal = new(attackNormal, new(() => behaviour.IsPlayerBeside() && behaviour.IsChasable()));
            // NodeTransition toRoam = new(walk, new(() => behaviour.timer.IsFinished && !behaviour.IsPlayerNearby() && behaviour.IsChasable()));

            StateNode.Creator<Nodes.Common.Empty>.Using(empty).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toAttackNormal).Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Idle>.Using(idle).WithTransition(toRoam).WithTransition(toChasing).WithTransition(toAttackNormal).WithAnimation("mob_idle").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Walk>.Using(chasing).WithTransition(toIdle).WithTransition(toAttackNormal).WithAnimation("mob_walk").Accomplish(controller);            
            StateNode.Creator<Nodes.Enemy.Attack>.Using(attackNormal).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toEmpty).WithAnimation("mob_attack").Accomplish(controller);            
            StateNode.Creator<Nodes.Enemy.Walk>.Using(roam).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toAttackNormal).WithAnimation("mob_walk").Accomplish(controller);
        }
    }
}
