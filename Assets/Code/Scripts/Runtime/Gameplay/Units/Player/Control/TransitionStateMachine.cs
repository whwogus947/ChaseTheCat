using UnityEngine;

namespace Com2usGameDev
{
    public class TransitionStateMachine : UnitStateMachine
    {
        public bool isRunning;

        protected override void Initialize()
        {
            behaviour = GetComponent<PlayerBehaviour>();
            states = new()
            {
                { typeof(PlayerStates.Idle), new PlayerStates.Idle(behaviour.idleState.AnimationHashName) },
                { typeof(PlayerStates.Walk), new PlayerStates.Walk(behaviour.walkState.AnimationHashName) },
                { typeof(PlayerStates.Run), new PlayerStates.Run(behaviour.runState.AnimationHashName) },
            };
            currentStateType = typeof(PlayerStates.Idle);
        }

        public void MovementState(float velocityX)
        {
            if (!behaviour.direction.isControllable)
                return;

            bool isMoving = Mathf.Abs(velocityX) > 0;

            if (isMoving && currentStateType != typeof(PlayerStates.Run) && !isRunning)
                ChangeState(typeof(PlayerStates.Walk));
            else if (isMoving && currentStateType == typeof(PlayerStates.Run) || (isRunning && isMoving))
                ChangeState(typeof(PlayerStates.Run));
            else
                ChangeState(typeof(PlayerStates.Idle));
        }
    }
}
