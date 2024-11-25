using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com2usGameDev
{
    [RequireComponent(typeof(PlayerBehaviour), typeof(PlayerStateMachine))]
    public class PlayerController : MonoBehaviour
    {
        public ClampedVector2dSO direction;
        public InputControllerSO inputController;

        private PlayerStateMachine stateMachine;
        private TransitionStateMachine transitionMachine;

        void Start()
        {
            inputController.Input.Player.Jump.performed += OnPressJump;
            inputController.Input.Player.Jump.canceled += OnStartJump;
            inputController.Input.Player.Attack.performed += OnAttack;
            inputController.Input.Player.Dash.performed += OnDash;
            inputController.Input.Player.Run.performed += OnRunStart;
            inputController.Input.Player.Run.canceled += OnRunEnd;

            stateMachine = GetComponent<PlayerStateMachine>();
            transitionMachine = GetComponent<TransitionStateMachine>();
        }

        void Update()
        {
            direction.Vector2 = inputController.Input.Player.Transit.ReadValue<Vector2>();
            transitionMachine.MovementState(direction.X);
            stateMachine.OnUpdate();
        }

        private void OnRunEnd(InputAction.CallbackContext context)
        {
            transitionMachine.ChangeState(typeof(PlayerStates.Idle));
        }

        private void OnRunStart(InputAction.CallbackContext context)
        {
            transitionMachine.ChangeState(typeof(PlayerStates.Run));
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            // ChangeState(typeof(PlayerStates.Jump));
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            // ChangeState(typeof(PlayerStates.Jump));
        }

        private void OnStartJump(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(typeof(PlayerStates.Jump));
        }

        private void OnPressJump(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(typeof(PlayerStates.JumpCharging));
        }
    }
}
