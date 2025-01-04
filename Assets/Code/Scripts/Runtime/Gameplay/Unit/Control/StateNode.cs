using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public enum State
    {
        Basic,
        Movement,
        Action,
    }

    public abstract class StateNode : IState
    {
        public State NodeState { get; private set; }
        public int AnimationHash { get; private set; }
        public UnityAction OnEnterAction { get; private set; }

        private readonly List<ITransition> transitions;

        public StateNode()
        {
            transitions = new();
        }

        public void SetNode(State state)
        {
            this.NodeState = state;
        }

        public virtual void OnInitialize(UnitBehaviour unit)
        {
            
        }

        public abstract void OnEnter(UnitBehaviour unit);

        public abstract void OnExit(UnitBehaviour unit);

        public abstract void OnUpdate(UnitBehaviour unit);

        protected void AddTransition(ITransition transition)
        {
            transitions.Add(transition);
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

        public void Accomplish(IStateAddible addible)
        {
            addible.AddState(this);
        }

        public void RemoveTransition(Type state)
        {
            if (transitions.Exists(x => x.To.GetType() == state))
                transitions.Remove(transitions.Find(x => x.To.GetType() == state));
        }

        public class Creator<T> where T : StateNode, new()
        {
            private T node;

            public static Creator<T> CreateType(State state)
            {
                var builder = new Creator<T>
                {
                    node = new()
                };
                builder.node.SetNode(state);
                return builder;
            }

            public static Creator<T> Using(T state)
            {
                var builder = new Creator<T>
                {
                    node = state
                };
                return builder;
            }

            public Creator<T> WithTransition(ITransition transition)
            {
                node.AddTransition(transition);
                return this;
            }

            public Creator<T> WithAnimation(string animName)
            {
                node.AnimationHash = Animator.StringToHash(animName);
                return this;
            }

            public T InProgress() => node;

            public T Accomplish(IStateAddible addible)
            {
                addible.AddState(node);
                return node;
            }

            public T WithAction(UnityAction @action)
            {
                node.OnEnterAction = @action;
                return node;
            }

            public void AccomplishOnly(IStateAddible addible, List<SpecialSO> specials)
            {
                if (specials.Exists(x => x.BehaviourType == typeof(T)))
                {
                    addible.AddState(node);
                    return;
                }
            }
        }
    }

    public abstract class SkillNode : StateNode
    {
        protected const string abilityType = nameof(SkillAbilitySO);
        protected abstract string SkillName { get; }
        protected SkillAbilitySO skillAbility;

        private float EP => skillAbility.epConsumption;
        private Func<bool> skillCondition;

        public bool IsUsable(AbilityController controller)
        {
            bool isUsable = HasAbility(controller) && skillAbility.IsReady && skillCondition();
            return isUsable;
        }

        public override void OnInitialize(UnitBehaviour unit)
        {
            skillCondition = () => HasEnoughEP(unit as PlayerBehaviour);
        }

        private bool HasEnoughEP(PlayerBehaviour unit)
        {
            return unit.EP >= EP;
        }

        protected bool HasAbility(AbilityController controller)
        {
            if (skillAbility == null)
                skillAbility = controller.GetAbility<SkillAbilitySO>(abilityType, SkillName);
            return skillAbility != null;
        }

        public override void OnEnter(UnitBehaviour unit)
        {
            var player = unit as PlayerBehaviour;
            if (skillAbility == null)
                skillAbility = player.abilityController.GetAbility<SkillAbilitySO>(abilityType, SkillName);

            OnSkillEnter(player);
            UseEP(player);
            skillAbility.ResetCoolTime();
        }

        private void UseEP(PlayerBehaviour player)
        {
            player.EP -= EP;
        }

        public abstract void OnSkillEnter(PlayerBehaviour player);
    }

    public class Nodes
    {
        public class Common
        {
            public class Empty : StateNode
            {
                public override void OnEnter(UnitBehaviour unit)
                {
                    
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    
                }

                public override void OnUpdate(UnitBehaviour unit)
                {

                }
            }
        }

        public class Player
        {
            public class Idle : StateNode
            {
                private PlayerBehaviour playerBehaviour;

                public override void OnEnter(UnitBehaviour unit)
                {
                    if (playerBehaviour == null)
                        playerBehaviour = unit as PlayerBehaviour;

                    // Debug.Log("Idle");
                    unit.Controllable = true;
                    unit.SetAnimation("IsWalking", false);
                    unit.SetAnimation("IsRunning", false);
                    unit.SetAnimation("IsOnGround", true);
                    unit.SetTransitionPower(0);
                    unit.TranslateX();
                }

                public override void OnExit(UnitBehaviour unit)
                {

                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    playerBehaviour.EP += Time.deltaTime * 1.5f;
                }
            }

            public class Walk : StateNode
            {
                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.SetAnimation("IsWalking", true);
                    unit.SetAnimation("IsRunning", false);
                    unit.SetAnimation("IsOnGround", true);
                    unit.SetTransitionPower(unit.walk);
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    unit.SetAnimation("IsWalking", false);
                    unit.SetAnimation("IsRunning", false);
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateX();
                }
            }

            public class Run : StateNode
            {
                private PlayerBehaviour playerBehaviour;

                public override void OnEnter(UnitBehaviour unit)
                {
                    if (playerBehaviour == null)
                        playerBehaviour = unit as PlayerBehaviour;

                    unit.SetAnimation("IsWalking", false);
                    unit.SetAnimation("IsRunning", true);
                    unit.SetAnimation("IsOnGround", true);
                    unit.SetTransitionPower(unit.run);
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    unit.SetAnimation("IsWalking", false);
                    unit.SetAnimation("IsRunning", false);
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateX();
                    playerBehaviour.EP -= Time.deltaTime * 6f;
                }
            }

            public class JumpCharging : StateNode
            {
                private PlayerBehaviour playerBehaviour;

                public override void OnEnter(UnitBehaviour unit)
                {
                    playerBehaviour = unit as PlayerBehaviour;
                    unit.PlayAnimation(AnimationHash, 0.2f);
                    unit.SetAnimation("IsOnGround", true);
                    unit.chargePower = 0;
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    unit.SetAnimation("IsOnGround", false);
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.SetTransitionPower(unit.jumpCharging);
                    unit.TranslateX();
                    unit.chargePower += Time.deltaTime;
                    playerBehaviour.jumpGauge.SetValue(unit.chargePower);
                }
            }

            public class OnAir : StateNode
            {
                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.SetAnimation("IsOnGround", false);
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    (unit as PlayerBehaviour).CalculateFallDamage();
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    
                }
            }

            public class Jump : StateNode
            {
                private PlayerBehaviour playerBehaviour;

                public override void OnEnter(UnitBehaviour unit)
                {
                    if (playerBehaviour == null)
                        playerBehaviour = unit as PlayerBehaviour;

                    playerBehaviour.EP -= Time.deltaTime * 5f;

                    unit.Jump();
                    unit.SetAnimation("IsOnGround", false);
                    unit.CaptureDirection(unit.jumpX);
                    unit.Controllable = false;
                    unit.PlaySound(playerBehaviour.playerSFX.jump);
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    unit.Controllable = true;
                    unit.SetAnimation("IsOnGround", true);

                    (unit as PlayerBehaviour).CalculateFallDamage();
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateFixedX();
                }
            }

            public class DoubleJump : SkillNode
            {
                protected override string SkillName => nameof(DoubleJumpSkillSO);

                public override void OnExit(UnitBehaviour unit)
                {
                    unit.Controllable = true;
                    unit.SetAnimation("IsOnGround", true);

                    (unit as PlayerBehaviour).CalculateFallDamage();
                }

                public override void OnSkillEnter(PlayerBehaviour player)
                {
                    player.PlaySound(player.playerSFX.doubleJump);
                    player.ResetVelocity();
                    player.Jump(0.75f);
                    player.SetAnimation("IsOnGround", false);
                    player.CaptureDirection(player.jumpX);
                    player.VisualizeFX(skillAbility.fx);
                    player.Controllable = false;
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateFixedX();
                }
            }

            public class StaticFlight : SkillNode
            {
                protected override string SkillName => nameof(StaticFlightSkillSO);

                public override void OnExit(UnitBehaviour unit)
                {
                    if (unit is PlayerBehaviour behaviour)
                    {
                        behaviour.RestartRigidbody();
                        unit.SetAnimation("IsHolding", false);
                    }
                }

                public override void OnSkillEnter(PlayerBehaviour player)
                {
                    player.PlaySound(player.playerSFX.staticFlight);
                    player.ResetMaxHeight();

                    player.VisualizeFX(skillAbility.fx);

                    player.PlayAnimation(AnimationHash, 0.2f);
                    player.InvalidateRigidbody();
                    player.SetAnimation("IsHolding", true);
                }

                public override void OnUpdate(UnitBehaviour unit)
                {

                }
            }

            public class Dash : SkillNode
            {
                protected override string SkillName => nameof(DashSkillSO);

                public override void OnExit(UnitBehaviour unit)
                {
                    
                }

                public override void OnSkillEnter(PlayerBehaviour player)
                {
                    player.PlaySound(player.playerSFX.dash);
                    player.PlayAnimation(AnimationHash, 0.2f);
                    player.SetTransitionPower(player.dash);
                    player.CaptureDirection();
                    player.VisualizeFX(skillAbility.fx);
                    OnEnterAction();
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateFixedX();
                }
            }

            public class AttackNormal : StateNode
            {
                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.Attack();
                }

                public override void OnExit(UnitBehaviour unit)
                {

                }

                public override void OnUpdate(UnitBehaviour unit)
                {

                }
            }

            public class Rope : StateNode
            {
                private PlayerBehaviour playerBehaviour;

                public override void OnEnter(UnitBehaviour unit)
                {
                    Debug.Log("Rope");
                    unit.Controllable = false;
                    unit.SetTransitionPower(0);
                    unit.TranslateX();
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    unit.Controllable = true;
                }
                public override void OnUpdate(UnitBehaviour unit)
                {
                    
                }
            }
        }

        public class Enemy
        {
            public class Idle : StateNode
            {
                private float idleTimer = 0f;

                public bool IsTimeOver() => idleTimer <= 0;

                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.SetTransitionPower(0);
                    unit.PlayAnimation(AnimationHash, 0.2f);
                    unit.TranslateX();
                    idleTimer = UnityEngine.Random.Range(2, 5);
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    // idleTimer = 0f;
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    idleTimer -= Time.deltaTime;
                }

                public void ResetTimer() => idleTimer = 0f;
            }

            public class Roam : StateNode
            {
                private float roamTimer = 0;

                public bool IsTimeOver() => roamTimer <= 0;

                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.SetTransitionPower(unit.walk);
                    unit.PlayAnimation(AnimationHash, 0.2f);
                    roamTimer = UnityEngine.Random.Range(2, 5);
                    (unit as MonsterBehaviour).ResetVelocityDirection();
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateX();
                    roamTimer -= Time.deltaTime;
                }

                public void ResetTimer() => roamTimer = 0f;                
            }

            public class Chase : StateNode
            {
                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.SetTransitionPower(unit.walk * 1.4f);
                    unit.PlayAnimation(AnimationHash, 0.2f);
                    (unit as MonsterBehaviour).OnFindPlayer();
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateX();
                }
            }

            public class NormalAttack : StateNode
            {
                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.timer = new(0.94f);
                    unit.SetTransitionPower(0);
                    unit.TranslateX();
                    // unit.PlayAnimation(AnimationHash, 0.2f);
                    unit.Attack();
                }

                public override void OnExit(UnitBehaviour unit)
                {

                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.timer.Tick();
                }
            }

            public class Sprint : StateNode
            {
                public bool IsReady => _isReady;

                private bool _isReady = true;

                public override void OnEnter(UnitBehaviour unit)
                {
                    unit.SetTransitionPower(unit.walk * 3f);
                    unit.PlayAnimation(AnimationHash, 0.2f);
                    if (IsReady)
                        Countdown(10f).Forget();
                }

                public override void OnExit(UnitBehaviour unit)
                {
                    
                }

                public override void OnUpdate(UnitBehaviour unit)
                {
                    unit.TranslateX();
                }

                private async UniTaskVoid Countdown(float time)
                {
                    _isReady = false;
                    await UniTask.WaitForSeconds(time);
                    _isReady = true;
                }
            }
        }
    }
}