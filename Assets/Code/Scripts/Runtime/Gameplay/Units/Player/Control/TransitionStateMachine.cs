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

        public void MovementState(float velocityX)
        {
            bool isMoving = Mathf.Abs(velocityX) > 0;
            if (IsCurrentStateType(typeof(PlayerStates.JumpCharging)))
                return;

            if (isMoving && currentStateType != typeof(PlayerStates.Run))
                ChangeState(typeof(PlayerStates.Walk));
            else if (currentStateType == typeof(PlayerStates.Run))
                ChangeState(typeof(PlayerStates.Run));
            else
                ChangeState(typeof(PlayerStates.Idle));
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
