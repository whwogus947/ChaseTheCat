using UnityEngine;

namespace Com2usGameDev
{
    public class TransitionStateMachine : UnitStateMachine
    {
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

        public override void OnUpdate()
        {
            states[currentStateType].OnUpdate(behaviour);
        }

        public void MovementState(float velocityX)
        {
            if (!behaviour.direction.isControllable)
                return;

            bool isMoving = Mathf.Abs(velocityX) > 0;

            if (isMoving && currentStateType != typeof(PlayerStates.Run) && !IsTransmittable)
                ChangeState(typeof(PlayerStates.Walk));
            else if (isMoving && currentStateType == typeof(PlayerStates.Run) || (IsTransmittable && isMoving))
                ChangeState(typeof(PlayerStates.Run));
            else
                ChangeState(typeof(PlayerStates.Idle));
        }
    }
}
