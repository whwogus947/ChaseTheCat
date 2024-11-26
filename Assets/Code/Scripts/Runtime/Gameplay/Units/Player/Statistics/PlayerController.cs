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
        public LayerMask groundLayer;

        private PlayerStateMachine stateMachine;
        private TransitionStateMachine transitionMachine;

        void Start()
        {
            direction.isControllable = true;

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
            direction.Vector2 = IsOnGround() ? inputController.Input.Player.Transit.ReadValue<Vector2>() : direction.Vector2;
            if (stateMachine.IsMovable())
                transitionMachine.MovementState(direction.X);

            stateMachine.OnUpdate();
            transitionMachine.OnUpdate();
        }

        private bool IsOnGround()
        {
            var rayHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.92f, 0, Vector2.down, 20f, groundLayer.value);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * distance, Color.yellow);
            return distance < 0.05f;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (IsOnGround())
            {
                transitionMachine.ChangeState(typeof(PlayerStates.Idle));
                stateMachine.ChangeState(typeof(PlayerStates.Idle));
                Debug.Log("On Ground");
            }
            else
            {
                direction.power = -direction.power;
                Debug.Log("Next To Wall");
            }
        }

        private void OnRunEnd(InputAction.CallbackContext context)
        {
            transitionMachine.IsTransmittable = false;
            transitionMachine.ChangeState(typeof(PlayerStates.Idle));
        }

        private void OnRunStart(InputAction.CallbackContext context)
        {
            transitionMachine.IsTransmittable = true;
            if (direction.isControllable)
                transitionMachine.ChangeState(typeof(PlayerStates.Run));
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            if (direction.X == 0)
                return;
            transitionMachine.ChangeState(typeof(PlayerStates.Idle));
            stateMachine.ChangeState(typeof(PlayerStates.Dash));
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            transitionMachine.ChangeState(typeof(PlayerStates.Idle));
            stateMachine.ChangeState(typeof(PlayerStates.NormalAttack));
        }

        private void OnStartJump(InputAction.CallbackContext context)
        {
            if (!IsOnGround())
                return;
            transitionMachine.ChangeState(typeof(PlayerStates.Idle));
            stateMachine.ChangeState(typeof(PlayerStates.Jump));
        }

        private void OnPressJump(InputAction.CallbackContext context)
        {
            if (!IsOnGround())
                return;
            stateMachine.ChangeState(typeof(PlayerStates.JumpCharging));
        }
    }
}
