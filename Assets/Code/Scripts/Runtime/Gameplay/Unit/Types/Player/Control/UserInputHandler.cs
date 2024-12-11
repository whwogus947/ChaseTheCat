using UnityEngine;
using UnityEngine.InputSystem;

namespace Com2usGameDev
{
    public class UserInputHandler : MonoBehaviour
    {
        public InputControllerSO inputController;
        public LinearStatSO direction;
        public BoolValueSO groundChecker;
        public BoolValueSO controllable;

        private PCInput input;
        private readonly Timer timer = new();
        private bool isDashPressed;

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
            input.Player.Jump.performed += OnPressJump;
            input.Player.Jump.canceled += OnReleaseJump;
            input.Player.Attack.performed += OnPressAttack;
        }

        private void OnPressJump(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.JumpCharging>(0.1f);
        }

        private void OnPressAttack(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.AttackNormal>(0.4f);
        }

        private void OnReleaseJump(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.Jump>(0.1f);
        }

        private void OnPressDash(InputAction.CallbackContext context)
        {
            isDashPressed = true;
            timer.StartTimer<Nodes.Dash>(0.7f);
        }

        private bool IsDashPressed()
        {
            if (isDashPressed && !timer.HasTimerExpired<Nodes.Dash>())
            {
                isDashPressed = false;
                return true;
            }
            return false;
        }

        public void BindInputToController(StateController controller)
        {
            var emptyMovement = StateNode.Creator<Nodes.Empty>.CreateType(State.Movement).InProgress();
            var idle = StateNode.Creator<Nodes.Idle>.CreateType(State.Movement).InProgress();
            var walk = StateNode.Creator<Nodes.Walk>.CreateType(State.Movement).InProgress();
            var run = StateNode.Creator<Nodes.Run>.CreateType(State.Movement).InProgress();
            var dash = StateNode.Creator<Nodes.Dash>.CreateType(State.Movement).InProgress();

            var emptyAction = StateNode.Creator<Nodes.Empty>.CreateType(State.Action).InProgress();
            var jumpCharging = StateNode.Creator<Nodes.JumpCharging>.CreateType(State.Action).InProgress();
            var jump = StateNode.Creator<Nodes.Jump>.CreateType(State.Action).InProgress();
            var attackNormal = StateNode.Creator<Nodes.AttackNormal>.CreateType(State.Action).InProgress();
            var onAir = StateNode.Creator<Nodes.OnAir>.CreateType(State.Action).InProgress();
            
            NodeTransition toIdle = new(idle, new(() => IsVelocityZero()));
            NodeTransition toEmptyMovement = new(emptyMovement, new(() => !controllable.Value));
            NodeTransition toWalk = new(walk, new(() => !IsVelocityZero() && input.Player.Run.phase == InputActionPhase.Waiting));
            NodeTransition toRun = new(run, new(() => !IsVelocityZero() && input.Player.Run.phase == InputActionPhase.Performed));
            NodeTransition toDash = new(dash, new(() => !IsVelocityZero() && IsDashPressed() && input.Player.Dash.phase == InputActionPhase.Performed));
            NodeTransition emptymovementToIdle = new(idle, new(() => controllable.Value && groundChecker.Value));
            NodeTransition dashToIdle = new(idle, new(() => timer.HasTimerExpired<Nodes.Dash>() && groundChecker.Value));

            NodeTransition toJumpCharging = new(jumpCharging, new(() => input.Player.Jump.phase == InputActionPhase.Performed && groundChecker.Value && !timer.HasTimerExpired<Nodes.JumpCharging>()));
            NodeTransition toJump = new(jump, new(() => input.Player.Jump.phase == InputActionPhase.Waiting));
            NodeTransition toOnAir = new(onAir, new(() => !groundChecker.Value));
            NodeTransition toEmptyAction = new(emptyAction, new(() => groundChecker.Value && timer.HasTimerExpired<Nodes.Jump>()));
            NodeTransition toAttack = new(attackNormal, new(() => input.Player.Attack.phase == InputActionPhase.Performed));
            NodeTransition attackToEmpty = new(emptyAction, new(() => input.Player.Attack.phase == InputActionPhase.Waiting && timer.HasTimerExpired<Nodes.AttackNormal>()));

            StateNode.Creator<Nodes.Empty>.Using(emptyMovement).WithTransition(emptymovementToIdle).Accomplish(controller);
            StateNode.Creator<Nodes.Idle>.Using(idle).WithTransition(toWalk).WithTransition(toEmptyMovement).WithTransition(toRun).WithAnimation("main-idle").Accomplish(controller);
            StateNode.Creator<Nodes.Walk>.Using(walk).WithTransition(toIdle).WithTransition(toEmptyMovement).WithTransition(toRun).WithTransition(toDash).WithAnimation("main_walk").Accomplish(controller);
            StateNode.Creator<Nodes.Run>.Using(run).WithTransition(toWalk).WithTransition(toEmptyMovement).WithTransition(toIdle).WithTransition(toDash).WithAnimation("main-run").Accomplish(controller);
            StateNode.Creator<Nodes.Dash>.Using(dash).WithTransition(dashToIdle).WithAnimation("main-dash").Accomplish(controller);

            StateNode.Creator<Nodes.Empty>.Using(emptyAction).WithTransition(toJumpCharging).WithTransition(toAttack).WithTransition(toOnAir).Accomplish(controller);
            StateNode.Creator<Nodes.JumpCharging>.Using(jumpCharging).WithTransition(toJump).WithTransition(toOnAir).WithAnimation("main-jumpcharging").Accomplish(controller);
            StateNode.Creator<Nodes.Jump>.Using(jump).WithTransition(toEmptyAction).Accomplish(controller);
            StateNode.Creator<Nodes.OnAir>.Using(onAir).WithTransition(toEmptyAction).Accomplish(controller);
            StateNode.Creator<Nodes.AttackNormal>.Using(attackNormal).WithAnimation("main-attack").WithTransition(attackToEmpty).Accomplish(controller);
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
