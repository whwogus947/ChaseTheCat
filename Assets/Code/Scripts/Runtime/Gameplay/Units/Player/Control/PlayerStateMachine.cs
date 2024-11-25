using System;
using System.Collections.Generic;
using UnityEngine;

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
            };
            currentStateType = typeof(PlayerStates.Idle);
        }

        public void OnUpdate()
        {
            OnUpdate(currentStateType);
            behaviour.OnUpdate();
        }

        private void OnUpdate(Type currentType)
        {
            if (currentType != currentStateType)
                ChangeState(currentType);

            states[currentStateType].OnUpdate(behaviour);
        }

        

        

        
    }

    public abstract class AnimState : IState
    {
        private readonly int hash;

        public AnimState(string animationHash)
        {
            hash = Animator.StringToHash(animationHash);
        }

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

            public override void OnEnter(UnitBehaviour unitStat)
            {
                unitStat.direction.power = unitStat.idleState.Value;
                PlayAnimation(unitStat);
                Debug.Log("Start Idle");
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                
            }
        }

        public class JumpCharging : AnimState
        {
            public JumpCharging(string animationHash) : base(animationHash)
            {
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                PlayAnimation(unitStat);
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                
            }
        }

        public class Walk : AnimState
        {
            public Walk(string animationHash) : base(animationHash)
            {
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                PlayAnimation(unitStat);
                unitStat.SetAnimationBool("IsWalking", true);
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
                // unitStat.MoveX(unitStat.walkState.Value * unitStat.walkState.direction.X);
            }
        }

        public class Run : AnimState
        {
            public Run(string animationHash) : base(animationHash)
            {
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                PlayAnimation(unitStat);
                unitStat.direction.power = unitStat.runState.Value;
                Debug.Log("Start Run");
                unitStat.SetAnimationBool("IsWalking", false);
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                unitStat.direction.power = 0;
                Debug.Log("End Run");
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                
            }
        }

        public class Jump : AnimState
        {
            public Jump(string animationHash) : base(animationHash)
            {
            }

            public override void OnEnter(UnitBehaviour unitStat)
            {
                PlayAnimation(unitStat);
                unitStat.Jump(unitStat.jumpState.Value);
            }

            public override void OnExit(UnitBehaviour unitStat)
            {
                
            }

            public override void OnUpdate(UnitBehaviour unitStat)
            {
                
            }
        }
    }
}
