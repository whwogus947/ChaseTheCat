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
            var normalAttack = StateNode.Creator<Nodes.Enemy.NormalAttack>.CreateType(State.Basic).InProgress();
            var throwAttack = StateNode.Creator<Nodes.Enemy.ThrowAttack>.CreateType(State.Basic).InProgress();
            var bowlAttack = StateNode.Creator<Nodes.Enemy.BowAttack>.CreateType(State.Basic).InProgress();

            NodeTransition toEmpty = new(empty, new(() => behaviour.timer.IsFinished));
            NodeTransition toIdle = new(idle, new(() => (roam.IsTimeOver() && idle.IsTimeOver()) || (behaviour.timer.IsFinished && !behaviour.IsPlayerBeside(behaviour.detectRange) && !behaviour.IsPlayerNearby()) || !behaviour.IsOnGround()));
            NodeTransition toChasing = new(chasing, new(() => behaviour.timer.IsFinished && !behaviour.IsPlayerBeside(behaviour.detectRange) && behaviour.IsPlayerNearby() && behaviour.IsOnGround()));
            NodeTransition toRoam = new(roam, new(() => idle.IsTimeOver() || !roam.IsTimeOver() && !behaviour.IsPlayerBeside(behaviour.detectRange) && !behaviour.IsPlayerNearby() && behaviour.IsOnGround()));
            NodeTransition roamToIdle = new(idle, new(() => roam.IsTimeOver() && behaviour.timer.IsFinished && !behaviour.IsPlayerBeside(behaviour.detectRange) && !behaviour.IsPlayerNearby() || !behaviour.IsOnGround()));
            NodeTransition toNormalAttack = new(normalAttack, new(() => behaviour.IsPlayerBeside(behaviour.detectRange) && behaviour.IsOnGround()));
            NodeTransition toThrowAttack = new(normalAttack, new(() => behaviour.IsPlayerBeside(behaviour.detectRange) && behaviour.IsOnGround()));
            NodeTransition toBowAttack = new(normalAttack, new(() => behaviour.IsPlayerBeside(behaviour.detectRange) && behaviour.IsOnGround()));
            NodeTransition toSprint = new(sprint, new(() => sprint.IsReady && behaviour.IsPlayerNearby(12f) && behaviour.IsOnGround()));

            StateNode.Creator<Nodes.Common.Empty>.Using(empty).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toBowAttack).WithTransition(toNormalAttack).WithTransition(toThrowAttack).Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Idle>.Using(idle).WithTransition(toRoam).WithTransition(toChasing).WithTransition(toBowAttack).WithTransition(toNormalAttack).WithTransition(toThrowAttack).WithAnimation("mob_idle").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Chase>.Using(chasing).WithTransition(toIdle).WithTransition(toSprint).WithTransition(toNormalAttack).WithTransition(toThrowAttack).WithTransition(toBowAttack).WithAnimation("mob_walk").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.NormalAttack>.Using(normalAttack).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toEmpty).Accomplish(controller);
            // StateNode.Creator<Nodes.Enemy.NormalAttack>.Using(normalAttack).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toEmpty).WithAnimation("mob_attack").AccomplishOnly(controller, specials);
            StateNode.Creator<Nodes.Enemy.Roam>.Using(roam).WithTransition(roamToIdle).WithTransition(toChasing).WithTransition(toNormalAttack).WithTransition(toThrowAttack).WithTransition(toBowAttack).WithAnimation("mob_walk").Accomplish(controller);
            StateNode.Creator<Nodes.Enemy.Sprint>.Using(sprint).WithTransition(toIdle).WithTransition(toNormalAttack).WithTransition(toThrowAttack).WithTransition(toBowAttack).WithAnimation("mob_run").AccomplishOnly(controller, specials);
            // StateNode.Creator<Nodes.Enemy.ThrowAttack>.Using(throwAttack).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toEmpty).WithAnimation("mob_attack2").AccomplishOnly(controller, specials);
            // StateNode.Creator<Nodes.Enemy.BowAttack>.Using(bowlAttack).WithTransition(toIdle).WithTransition(toChasing).WithTransition(toEmpty).WithAnimation("mob_attack3").AccomplishOnly(controller, specials);
        }
    }
}
