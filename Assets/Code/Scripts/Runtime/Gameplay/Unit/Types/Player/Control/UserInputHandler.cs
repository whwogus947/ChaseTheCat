using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com2usGameDev
{
    public class UserInputHandler : MonoBehaviour
    {
        public InputControllerSO inputController;
        public FloatValueSO velocityDirection;
        public BoolValueSO groundChecker;
        public BoolValueSO controllable;
        public AbilityController abilityController;

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
            input.Player.StaticFlight.performed += OnPressStaticFlight;
            input.Player.Interaction.performed += OnInteraction;
            // input.Player.Rope.performed += OnPressRope;
        }

        // private void OnPressRope(InputAction.CallbackContext context)
        // {
        //     timer.StartTimer<Nodes.Player.Rope>(1f);
        // }

        private void OnInteraction(InputAction.CallbackContext context)
        {
            if (gameObject.TryGetComponent(out PlayerBehaviour behaviour))
            {
                behaviour.MeetupNPC();
            }
        }

        private void OnPressStaticFlight(InputAction.CallbackContext context)
        {
            if (timer.HasTimerExpired<Nodes.Player.StaticFlight>())
            {
                timer.StartTimer<Nodes.Player.StaticFlight>(2f);
            }
            else
            {
                timer.EndTimer<Nodes.Player.StaticFlight>();
            }
        }

        private void OnPressJump(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.Player.JumpCharging>(0.1f);
        }

        private void OnPressAttack(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.Player.AttackNormal>(0.8f);
        }

        private void OnReleaseJump(InputAction.CallbackContext context)
        {
            timer.StartTimer<Nodes.Player.Jump>(0.1f);
        }

        private void OnPressDash(InputAction.CallbackContext context)
        {
            isDashPressed = true;
        }

        private void StartTimer<T>(float seconds)
        {
            timer.StartTimer<T>(seconds);
        }

        private WeaponAbilitySO GetWeapon(string weaponName)
        {
            return abilityController.GetAbility<WeaponAbilitySO>(nameof(WeaponAbilitySO), weaponName);
        }

        public void BindInputToController(StateController controller)
        {
            var emptyMovement = StateNode.Creator<Nodes.Common.Empty>.CreateType(State.Movement).InProgress();
            var idle = StateNode.Creator<Nodes.Player.Idle>.CreateType(State.Movement).InProgress();
            var walk = StateNode.Creator<Nodes.Player.Walk>.CreateType(State.Movement).InProgress();
            var run = StateNode.Creator<Nodes.Player.Run>.CreateType(State.Movement).InProgress();
            var dash = StateNode.Creator<Nodes.Player.Dash>.CreateType(State.Movement).InProgress();

            var emptyAction = StateNode.Creator<Nodes.Common.Empty>.CreateType(State.Action).InProgress();
            var jumpCharging = StateNode.Creator<Nodes.Player.JumpCharging>.CreateType(State.Action).InProgress();
            var jump = StateNode.Creator<Nodes.Player.Jump>.CreateType(State.Action).InProgress();
            var doubleJump = StateNode.Creator<Nodes.Player.DoubleJump>.CreateType(State.Action).InProgress();
            var attackNormal = StateNode.Creator<Nodes.Player.AttackNormal>.CreateType(State.Action).InProgress();
            var staticFlight = StateNode.Creator<Nodes.Player.StaticFlight>.CreateType(State.Action).InProgress();
            var onAir = StateNode.Creator<Nodes.Player.OnAir>.CreateType(State.Action).InProgress();
            // var rope = StateNode.Creator<Nodes.Player.Rope>.CreateType(State.Action).InProgress();

            NodeTransition toIdle = new(idle, new(() => IsVelocityZero()));
            NodeTransition toEmptyMovement = new(emptyMovement, new(() => !controllable.Value));
            NodeTransition toWalk = new(walk, new(() => !IsVelocityZero() && input.Player.Run.phase == InputActionPhase.Waiting));
            NodeTransition toRun = new(run, new(() => !IsVelocityZero() && input.Player.Run.phase == InputActionPhase.Performed));
            NodeTransition toDash = new(dash, new(() => dash.IsUsable(abilityController) && !IsVelocityZero() && isDashPressed && (input.Player.Dash.phase == InputActionPhase.Performed)));
            NodeTransition emptymovementToIdle = new(idle, new(() => controllable.Value && groundChecker.Value));
            NodeTransition dashToIdle = new(idle, new(() => timer.HasTimerExpired<Nodes.Player.Dash>() && groundChecker.Value));

            NodeTransition toJumpCharging = new(jumpCharging, new(() => input.Player.Jump.phase == InputActionPhase.Performed && groundChecker.Value && !timer.HasTimerExpired<Nodes.Player.JumpCharging>()));
            NodeTransition toJump = new(jump, new(() => input.Player.Jump.phase == InputActionPhase.Waiting));
            NodeTransition toDoubleJump = new(doubleJump, new(() => doubleJump.IsUsable(abilityController) && input.Player.Jump.phase == InputActionPhase.Performed));
            NodeTransition toOnAir = new(onAir, new(() => !groundChecker.Value));
            NodeTransition toEmptyAction = new(emptyAction, new(() => groundChecker.Value && timer.HasTimerExpired<Nodes.Player.Jump>()));
            NodeTransition toAttack = new(attackNormal, new(() => input.Player.Attack.phase == InputActionPhase.Performed));
            NodeTransition attackToEmpty = new(emptyAction, new(() => input.Player.Attack.phase == InputActionPhase.Waiting && timer.HasTimerExpired<Nodes.Player.AttackNormal>()));
            NodeTransition toStaticFlight = new(staticFlight, new(() => input.Player.StaticFlight.phase == InputActionPhase.Performed && !groundChecker.Value));
            NodeTransition staticFlightToEmptyAction = new(emptyAction, new(() => timer.HasTimerExpired<Nodes.Player.StaticFlight>()));
            // NodeTransition toRope = new(rope, new(() => (GetWeapon(nameof(HookSO)) as HookSO).IsUsing));
            // NodeTransition ropeToIdle = new(emptyAction, new(() => !(GetWeapon(nameof(HookSO)) as HookSO).IsUsing));

            // Movement
            StateNode.Creator<Nodes.Common.Empty>.Using(emptyMovement).
                WithTransition(emptymovementToIdle).
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.Idle>.Using(idle).
                WithTransition(toWalk).
                WithTransition(toEmptyMovement).
                WithTransition(toRun).
                WithAnimation("main-idle").
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.Walk>.Using(walk).
                WithTransition(toIdle).
                WithTransition(toEmptyMovement).
                WithTransition(toRun).
                WithTransition(toDash).
                WithAnimation("main_walk").
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.Run>.Using(run).
                WithTransition(toWalk).
                WithTransition(toEmptyMovement).
                WithTransition(toIdle).
                WithTransition(toDash).
                WithAnimation("main-run").
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.Dash>.Using(dash).
                WithTransition(dashToIdle).
                WithAnimation("main-dash").
                WithAction(() => StartTimer<Nodes.Player.Dash>(0.7f)).
                Accomplish(controller);

            // Action
            StateNode.Creator<Nodes.Common.Empty>.Using(emptyAction).
                // WithTransition(toRope).
                WithTransition(toJumpCharging).
                WithTransition(toAttack).
                WithTransition(toOnAir).
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.JumpCharging>.Using(jumpCharging).
                WithTransition(toJump).
                WithTransition(toOnAir).
                WithAnimation("main-jumpcharging").
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.Jump>.Using(jump).
                WithTransition(toEmptyAction).
                WithTransition(toDoubleJump).
                WithTransition(toStaticFlight).
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.DoubleJump>.Using(doubleJump).
                WithTransition(toEmptyAction).
                WithTransition(toStaticFlight).
                Accomplish(controller);
                
            StateNode.Creator<Nodes.Player.StaticFlight>.Using(staticFlight).
                WithTransition(staticFlightToEmptyAction).
                WithAnimation("main-holding").
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.OnAir>.Using(onAir).
                WithTransition(toStaticFlight).
                WithTransition(toEmptyAction).
                Accomplish(controller);

            StateNode.Creator<Nodes.Player.AttackNormal>.Using(attackNormal).
                WithAnimation("main-attack").
                WithTransition(attackToEmpty).
                Accomplish(controller);

            // StateNode.Creator<Nodes.Player.Rope>.Using(rope).
            //     WithTransition(ropeToIdle).
            //     Accomplish(controller);
        }

        private bool IsVelocityZero()
        {
            return Mathf.Abs(velocityDirection.Value) == 0;
        }

        public void UpdateInput()
        {
            var velocity = input.Player.Transit.ReadValue<Vector2>();
            velocityDirection.Value = velocity.x > 0 ? 1 : velocity.x < 0 ? -1 : 0;
        }

        private void OnDestroy()
        {
            inputController.Dispose();
        }
    }
}
