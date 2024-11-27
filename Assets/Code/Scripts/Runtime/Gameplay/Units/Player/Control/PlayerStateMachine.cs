using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerStateMachine : UnitStateMachine
    {
        protected override void Initialize()
        {
            behaviour = GetComponent<PlayerBehaviour>();
            states = new()
            {
                { typeof(PlayerStates.Idle), new PlayerStates.Idle(behaviour.idleState.AnimationHashName) },
                { typeof(PlayerStates.Jump), new PlayerStates.Jump(behaviour.jumpState.AnimationHashName) },
                { typeof(PlayerStates.JumpCharging), new PlayerStates.JumpCharging(behaviour.jumpChargingState.AnimationHashName) },
                { typeof(PlayerStates.Dash), new PlayerStates.Dash(behaviour.dashState.AnimationHashName, behaviour.dashState.Clip.length, () => ChangeState(typeof(PlayerStates.Idle))) },
                { typeof(PlayerStates.NormalAttack), new PlayerStates.NormalAttack(behaviour.normalAttackState.AnimationHashName, behaviour.normalAttackState.Clip.length, () => ChangeState(typeof(PlayerStates.Idle))) },
            };
            currentStateType = typeof(PlayerStates.Idle);
        }

        public bool IsMovable()
        {
            if (!behaviour.direction.isControllable)
                return false;
            behaviour.OnUpdate();
            return true;
        }

        private void OnUpdate(Type currentType)
        {
            if (currentType != currentStateType)
                ChangeState(currentType);

            states[currentStateType].OnUpdate(behaviour);
        }

        public override void OnUpdate()
        {
            states[currentStateType].OnUpdate(behaviour);
            // OnUpdate(currentStateType);
        }
    }

    public abstract class AnimState : IState
    {
        private readonly int hash;
        
        protected UnityAction exitEvent;

        public AnimState(string animationHash, UnityAction exitEvent = null)
        {
            hash = Animator.StringToHash(animationHash);
            this.exitEvent = exitEvent;
        }

        public abstract bool HasEnoughEP(UnitBehaviour unitStat);
        public abstract void OnEnter(UnitBehaviour unitStat);

        public abstract void OnExit(UnitBehaviour unitStat);

        public abstract void OnUpdate(UnitBehaviour unitStat);

        protected void PlayAnimation(UnitBehaviour unitStat)
        {
            unitStat.PlayAnimation(hash);
        }
    }

    public class PlayerStates
    {
        public class Idle : AnimState
        {
            public Idle(string animationHash) : base(animationHash)
            {
            }

            public override bool HasEnoughEP(UnitBehaviour unitStat)
            {
                return true;
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                unitStat.SetAnimationBool("IsOnGround", true);
                Debug.Log("Start Idle");
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                
            }
        }

        public class NormalAttack : AnimState
        {
            private float timer;
            private readonly float attackTime;

            public NormalAttack(string animationHash, float timer, UnityAction exitEvent = null) : base(animationHash, exitEvent)
            {
                attackTime = timer;
                this.timer = timer;
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                PlayAnimation(unitStat);
                timer = attackTime;
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;

                    if (timer <= 0)
                    {
                        exitEvent?.Invoke();
                    }
                }
            }

            public override bool HasEnoughEP(UnitBehaviour unitStat)
            {
                return true;
            }
        }

        public class Dash : AnimState
        {
            private float timer;
            private readonly float dashTime;

            public Dash(string animationHash, float timer, UnityAction exitEvent = null) : base(animationHash, exitEvent)
            {
                dashTime = timer;
                this.timer = timer;
            }

            public override bool HasEnoughEP(UnitBehaviour unitStat)
            {
                return true;
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                unitStat.direction.power = unitStat.dashState.Value * unitStat.direction.X;
                timer = dashTime;
                unitStat.direction.isControllable = false;
                PlayAnimation(unitStat);
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                unitStat.direction.isControllable = true;
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                if (timer > 0 && !unitStat.direction.isControllable)
                {
                    timer -= Time.deltaTime;
                    unitStat.MoveX(unitStat.direction.power);

                    if (timer <= 0)
                    {
                        unitStat.direction.isControllable = true;
                        exitEvent?.Invoke();
                    }
                }
            }
        }

        public class JumpCharging : AnimState
        {
            public JumpCharging(string animationHash) : base(animationHash)
            {
            }

            public override bool HasEnoughEP(UnitBehaviour unitStat)
            {
                return true;
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                unitStat.direction.power = unitStat.jumpChargingState.Value;
                PlayAnimation(unitStat);
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                unitStat.direction.power = unitStat.jumpChargingState.Value;
            }
        }

        public class Walk : AnimState
        {
            public Walk(string animationHash) : base(animationHash)
            {
            }

            public override bool HasEnoughEP(UnitBehaviour unitStat)
            {
                return true;
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                unitStat.SetAnimationBool("IsWalking", true);
                unitStat.SetAnimationBool("IsRun", false);
                unitStat.direction.power = unitStat.walkState.Value;
                Debug.Log("Start Walk");
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                unitStat.direction.power = 0;
                unitStat.SetAnimationBool("IsWalking", false);
                Debug.Log("End Walk");
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                
            }
        }

        public class Run : AnimState
        {
            public Run(string animationHash) : base(animationHash)
            {
            }

            public override bool HasEnoughEP(UnitBehaviour unitStat)
            {
                if (unitStat.epDeltaBar.value > unitStat.runState.EPDeltaConsume * Time.deltaTime)
                {
                    unitStat.epDeltaBar.value -= unitStat.runState.EPDeltaConsume * Time.deltaTime;
                    return true;
                }
                return false;
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                unitStat.direction.power = unitStat.runState.Value;
                Debug.Log("Start Run");
                unitStat.SetAnimationBool("IsRun", true);
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                unitStat.direction.power = 0;
                Debug.Log("End Run");
                unitStat.SetAnimationBool("IsWalking", false);
                unitStat.SetAnimationBool("IsRun", false);
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                unitStat.epDeltaBar.TryUse(unitStat.runState.EPDeltaConsume);
            }
        }

        public class Jump : AnimState
        {
            public Jump(string animationHash) : base(animationHash)
            {
            }

            public override bool HasEnoughEP(UnitBehaviour unitStat)
            {
                return true;
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                PlayAnimation(unitStat);
                unitStat.Jump(unitStat.jumpState.Value);
                unitStat.direction.power = unitStat.walkState.Value * unitStat.direction.X;
                unitStat.direction.isControllable = false;
                unitStat.SetAnimationBool("IsOnGround", false);
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                unitStat.direction.isControllable = true;
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                if (unitStat.IsCollideWithWall())
                    unitStat.direction.power = -unitStat.walkState.Value * unitStat.ChildDirection();
                unitStat.MoveX(unitStat.direction.power);
            }
        }
    }
}
