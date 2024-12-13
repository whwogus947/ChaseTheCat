using System.Collections.Generic;
using UnityEngine;

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

        private readonly List<ITransition> transitions;

        public StateNode()
        {
            transitions = new();
        }

        public void SetNode(State state)
        {
            this.NodeState = state;
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
        }
    }

    public class Nodes
    {
        #region Player
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

        public class Idle : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
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
            public override void OnEnter(UnitBehaviour unit)
            {
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
            }
        }

        public class JumpCharging : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                Debug.Log("CHARGING!");
                unit.PlayAnimation(AnimationHash, 0.2f);
                unit.SetAnimation("IsOnGround", true);
            }

            public override void OnExit(UnitBehaviour unit)
            {
                unit.SetAnimation("IsOnGround", false);
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                unit.SetTransitionPower(unit.jumpCharging);
                unit.TranslateX();
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

            }

            public override void OnUpdate(UnitBehaviour unit)
            {

            }
        }

        public class Jump : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                Debug.Log("JUMP");
                unit.Jump();
                unit.SetAnimation("IsOnGround", false);
                unit.CaptureDirection(unit.jumpX);
                unit.Controllable = false;
            }

            public override void OnExit(UnitBehaviour unit)
            {
                unit.Controllable = true;
                unit.SetAnimation("IsOnGround", true);
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                unit.TranslateFixedX();
            }
        }

        public class DoubleJump : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                Debug.Log("DOUBLE JUMP");
                unit.Jump();
                unit.SetAnimation("IsOnGround", false);
                unit.CaptureDirection(unit.jumpX);
                unit.Controllable = false;
            }

            public override void OnExit(UnitBehaviour unit)
            {
                unit.Controllable = true;
                unit.SetAnimation("IsOnGround", true);
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                unit.TranslateFixedX();
            }
        }

        public class StaticFlight : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                if (unit is PlayerBehaviour behaviour)
                {
                    unit.PlayAnimation(AnimationHash, 0.2f);
                    behaviour.InvalidateRigidbody();
                    unit.SetAnimation("IsHolding", true);
                }
            }

            public override void OnExit(UnitBehaviour unit)
            {
                if (unit is PlayerBehaviour behaviour)
                {
                    behaviour.RegenerateRigidbody();
                    unit.SetAnimation("IsHolding", false);
                }
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                
            }
        }

        public class Dash : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                unit.PlayAnimation(AnimationHash, 0.2f);
                unit.SetTransitionPower(unit.dash);
                unit.CaptureDirection();
                unit.UseVFX();
            }

            public override void OnExit(UnitBehaviour unit)
            {

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
                unit.PlayAnimation(AnimationHash, 0.2f);
                unit.Attack();
            }

            public override void OnExit(UnitBehaviour unit)
            {

            }

            public override void OnUpdate(UnitBehaviour unit)
            {

            }
        }

        public class AttackRanged : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnExit(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }
        }
        #endregion
        #region Monster
        public class MonIdle : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                Debug.Log("Idle");
                unit.SetTransitionPower(0);
                unit.PlayAnimation(AnimationHash, 0.2f);
                unit.TranslateX();
            }

            public override void OnExit(UnitBehaviour unit)
            {
                
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                
            }
        }

        public class MonWalk : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                Debug.Log("Walk");
                unit.SetTransitionPower(unit.walk);
                unit.PlayAnimation(AnimationHash, 0.2f);
            }

            public override void OnExit(UnitBehaviour unit)
            {
                
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                unit.TranslateX();
            }
        }

        public class MonAttack : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                Debug.Log("MonAttack");
                unit.timer = new(0.94f);
                unit.SetTransitionPower(0);
                unit.TranslateX();
                unit.PlayAnimation(AnimationHash, 0.2f);
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

        public class MonRun : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnExit(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MonDead : StateNode
        {
            public override void OnEnter(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnExit(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }

            public override void OnUpdate(UnitBehaviour unit)
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion
    }
}