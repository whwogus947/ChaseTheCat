using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [RequireComponent(typeof(MonsterBehaviour))]
    public class MonsterController : MonoBehaviour
    {
        public List<SpecialSO> specials;

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
            var chasing = StateNode.Creator<Nodes.Enemy.Chase>.CreateType(State.Basic).InProgress();
            var roam = StateNode.Creator<Nodes.Enemy.Roam>.CreateType(State.Basic).InProgress();
            var sprint = StateNode.Creator<Nodes.Enemy.Sprint>.CreateType(State.Basic).InProgress();
            var attackNormal = StateNode.Creator<Nodes.Enemy.Attack>.CreateType(State.Basic).InProgress();
            // var dead = StateNode.Creator<Nodes.MonDead>.CreateType(State.Basic).InProgress();

            NodeTransition toEmpty = new(empty, new(() => behaviour.timer.IsFinished));
            // NodeTransition toIdle = new(idle, new(() => !roam.IsRoaming() && behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && !behaviour.IsPlayerNearby() || (!behaviour.IsChasable() && !roam.IsRoaming())));
            NodeTransition toIdle = new(idle, new(() => !roam.IsRoaming() && behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && !behaviour.IsPlayerNearby() || !behaviour.IsOnGround()));
            NodeTransition toChasing = new(chasing, new(() => behaviour.timer.IsFinished && !behaviour.IsPlayerBeside() && behaviour.IsPlayerNearby() && behaviour.IsOnGround()));
            NodeTransition toRoam = new(roam, new(() => idle.IsWandering() && !behaviour.IsPlayerBeside() && !behaviour.IsPlayerNearby() && behaviour.IsOnGround()));
            NodeTransition toAttackNormal = new(attackNormal, new(() => behaviour.IsPlayerBeside() && behaviour.IsOnGround()));
            NodeTransition toSprint = new(sprint, new(() => sprint.IsReady && behaviour.IsPlayerNearby(12f) && behaviour.IsOnGround()));

            StateNode.Creator<Nodes.Common.Empty>.Using(empty).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toAttackNormal).Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Idle>.Using(idle).WithTransition(toRoam).WithTransition(toChasing).WithTransition(toAttackNormal).WithAnimation("mob_idle").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Chase>.Using(chasing).WithTransition(toIdle).WithTransition(toSprint).WithTransition(toAttackNormal).WithAnimation("mob_walk").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Attack>.Using(attackNormal).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toEmpty).WithAnimation("mob_attack").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Roam>.Using(roam).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toAttackNormal).WithAnimation("mob_walk").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Sprint>.Using(sprint).WithTransition(toIdle).WithTransition(toAttackNormal).WithAnimation("mob_run").AccomplishOnly(controller, specials);
        }
    }
}
