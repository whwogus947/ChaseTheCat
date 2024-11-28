using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com2usGameDev.Dev
{
    public class UserInputHandler : MonoBehaviour
    {
        public InputControllerSO inputController;
        public LinearStatSO direction;
        public BoolValueSO groundChecker;
        public BoolValueSO controllable;

        private PCInput input;
        private readonly Timer timer = new();

        private void Awake()
        {
            input = inputController.GetOrCreate();
        }

        private void Start()
        {
            UserKeyPress();
        }

        private void UserKeyPress()
        {
            input.Player.Dash.performed += OnPressDash;
            input.Player.Jump.canceled += OnReleaseJump;
        }

        private void OnReleaseJump(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.Jump>(0.1f);
        }

        private void OnPressDash(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.Dash>(0.7f);
        }

        public void BindInputToController(StateController controller)
        {
            var idle = StateNode.Creator<Nodes.Idle>.CreateType(State.Movement).InProgress();
            var walk = StateNode.Creator<Nodes.Walk>.CreateType(State.Movement).InProgress();
            var run = StateNode.Creator<Nodes.Run>.CreateType(State.Movement).InProgress();
            var dash = StateNode.Creator<Nodes.Dash>.CreateType(State.Movement).InProgress();
            var emptyMovement = StateNode.Creator<Nodes.Empty>.CreateType(State.Movement).InProgress();

            var emptyAction = StateNode.Creator<Nodes.Idle>.CreateType(State.Action).InProgress();
            var jumpCharging = StateNode.Creator<Nodes.JumpCharging>.CreateType(State.Action).InProgress();
            var jump = StateNode.Creator<Nodes.Jump>.CreateType(State.Action).InProgress();
            
            NodeTransition toIdle = new(idle, new(() => IsVelocityZero()));
            NodeTransition toEmptyMovement = new(emptyMovement, new(() => !controllable.Value));
            NodeTransition toWalk = new(walk, new(() => !IsVelocityZero() && input.Player.Run.phase == InputActionPhase.Waiting));
            NodeTransition toRun = new(run, new(() => !IsVelocityZero() && input.Player.Run.phase == InputActionPhase.Performed));
            NodeTransition toDash = new(dash, new(() => !IsVelocityZero() && input.Player.Dash.phase == InputActionPhase.Performed));
            NodeTransition emptymovementToIdle = new(idle, new(() => controllable.Value));
            NodeTransition dashToIdle = new(idle, new(() => timer.HasTimerExpired<Nodes.Dash>()));

            NodeTransition toJumpCharging = new(jumpCharging, new(() => input.Player.Jump.phase == InputActionPhase.Performed && groundChecker.Value));
            NodeTransition toJump = new(jump, new(() => (input.Player.Jump.phase == InputActionPhase.Waiting) || (!groundChecker.Value && input.Player.Jump.phase == InputActionPhase.Performed)));
            NodeTransition toEmptyAction = new(emptyAction, new(() => groundChecker.Value && timer.HasTimerExpired<Nodes.Jump>()));

            StateNode.Creator<Nodes.Idle>.Using(idle).WithTransition(toWalk).WithTransition(toEmptyMovement).WithTransition(toRun).WithAnimation("main-idle").Accomplish(controller);
            StateNode.Creator<Nodes.Walk>.Using(walk).WithTransition(toIdle).WithTransition(toEmptyMovement).WithTransition(toRun).WithTransition(toDash).WithAnimation("main_walk").Accomplish(controller);
            StateNode.Creator<Nodes.Run>.Using(run).WithTransition(toWalk).WithTransition(toEmptyMovement).WithTransition(toIdle).WithTransition(toDash).WithAnimation("main-run").Accomplish(controller);
            StateNode.Creator<Nodes.Dash>.Using(dash).WithTransition(dashToIdle).WithAnimation("main-dash").Accomplish(controller);
            StateNode.Creator<Nodes.Empty>.Using(emptyMovement).WithTransition(emptymovementToIdle).Accomplish(controller);

            StateNode.Creator<Nodes.Idle>.Using(emptyAction).WithTransition(toJumpCharging).Accomplish(controller);
            StateNode.Creator<Nodes.JumpCharging>.Using(jumpCharging).WithTransition(toJump).WithAnimation("main-jumpcharging").Accomplish(controller);
            StateNode.Creator<Nodes.Jump>.Using(jump).WithTransition(toEmptyAction).Accomplish(controller);
        }

        private bool IsVelocityZero()
        {
            return Mathf.Abs(direction.value) == 0;
        }

        public void UpdateInput()
        {
            var velocity = input.Player.Transit.ReadValue<Vector2>();
            direction.value = velocity.x > 0 ? 1 : velocity.x < 0 ? -1 : 0;
        }

        private void OnDestroy()
        {
            inputController.Dispose();
        }
    }
}
