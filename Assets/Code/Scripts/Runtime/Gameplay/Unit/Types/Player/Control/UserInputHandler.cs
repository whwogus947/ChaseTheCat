using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionPhase;
using static Com2usGameDev.Nodes.Player;

namespace Com2usGameDev
{
    public class UserInputHandler : MonoBehaviour
    {
        public InputControllerSO inputController;

        private AbilityController abilityController;
        private ControlData controlData;
        private PlayerBehaviour player;

        private PCInput input;
        private readonly Timer timer = new();
        private bool isDashPressed;
        private PCInput.PlayerActions UserInput => input.Player;

        private void Awake()
        {
            input = inputController.GetOrCreate();
            player = gameObject.GetComponentInEntire<PlayerBehaviour>();
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
        }

        private void OnInteraction(InputAction.CallbackContext context)
        {
            if (gameObject.TryGetComponent(out PlayerBehaviour behaviour))
            {
                behaviour.Interaction();
            }
        }

        private void OnPressStaticFlight(InputAction.CallbackContext context)
        {
            if (timer.HasTimerExpired<StaticFlight>())
            {
                timer.StartTimer<StaticFlight>(2f);
            }
            else
            {
                timer.EndTimer<StaticFlight>();
            }
        }

        private void OnPressJump(InputAction.CallbackContext context)
        {
            timer.StartTimer<JumpCharging>(0.1f);
        }

        private void OnPressAttack(InputAction.CallbackContext context)
        {
            timer.StartTimer<AttackNormal>(player.Delay);
        }

        private void OnReleaseJump(InputAction.CallbackContext context)
        {
            timer.StartTimer<Jump>(0.1f);
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

        private bool IsMovableGround() => controlData.controllable && controlData.groundValue;

        private bool IsPerformed(InputAction inputAction, InputActionPhase inputPhase = Performed)
        {
            return inputAction.phase == inputPhase;
        }

        private bool IsOnGround() => controlData.groundValue;

        public void BindInputToController(StateController controller, ControlData controlData, AbilityController abilityController)
        {
            this.controlData = controlData;
            this.abilityController = abilityController;

            #region Definition
            var emptyMovement = StateNode.Creator<Nodes.Common.Empty>.CreateType(State.Movement).InProgress();
            var idle = StateNode.Creator<Idle>.CreateType(State.Movement).InProgress();
            var walk = StateNode.Creator<Walk>.CreateType(State.Movement).InProgress();
            var run = StateNode.Creator<Run>.CreateType(State.Movement).InProgress();
            var dash = StateNode.Creator<Dash>.CreateType(State.Movement).InProgress();

            var emptyAction = StateNode.Creator<Nodes.Common.Empty>.CreateType(State.Action).InProgress();
            var jumpCharging = StateNode.Creator<JumpCharging>.CreateType(State.Action).InProgress();
            var jump = StateNode.Creator<Jump>.CreateType(State.Action).InProgress();
            var doubleJump = StateNode.Creator<DoubleJump>.CreateType(State.Action).InProgress();
            var attackNormal = StateNode.Creator<AttackNormal>.CreateType(State.Action).InProgress();
            var staticFlight = StateNode.Creator<StaticFlight>.CreateType(State.Action).InProgress();
            var onAir = StateNode.Creator<OnAir>.CreateType(State.Action).InProgress();
            #endregion

            #region Conditions
            NodeTransition toIdle = new(idle,
                new(IsStopped));

            NodeTransition toEmptyMovement = new(emptyMovement,
                new(() => !controlData.controllable));

            NodeTransition toWalk = new(walk,
                new(() => !IsStopped() && IsPerformed(UserInput.Run, Waiting)));

            NodeTransition toRun = new(run,
                new(() => !IsStopped() && IsPerformed(UserInput.Run)));

            NodeTransition toDash = new(dash,
                new(() => dash.IsUsable(abilityController) && !IsStopped() && isDashPressed && IsPerformed(UserInput.Dash)));

            NodeTransition emptymovementToIdle = new(idle,
                new(() => IsMovableGround()));

            NodeTransition dashToIdle = new(idle,
                new(() => timer.HasTimerExpired<Dash>() && IsOnGround()));

            NodeTransition toJumpCharging = new(jumpCharging,
                new(() => IsPerformed(UserInput.Jump) && IsOnGround() && !timer.HasTimerExpired<JumpCharging>()));

            NodeTransition toJump = new(jump,
                new(() => IsPerformed(UserInput.Jump, Waiting)));

            NodeTransition toDoubleJump = new(doubleJump,
                new(() => doubleJump.IsUsable(abilityController) && IsPerformed(UserInput.Jump)));

            NodeTransition toOnAir = new(onAir,
                new(() => !IsOnGround()));

            NodeTransition toEmptyAction = new(emptyAction,
                new(() => IsOnGround() && timer.HasTimerExpired<Jump>()));

            NodeTransition toAttack = new(attackNormal,
                new(() => IsPerformed(UserInput.Attack)));

            NodeTransition attackToEmpty = new(emptyAction,
                new(() => IsPerformed(UserInput.Attack, Waiting) && timer.HasTimerExpired<AttackNormal>()));

            NodeTransition toStaticFlight = new(staticFlight,
                new(() => staticFlight.IsUsable(abilityController) && IsPerformed(UserInput.StaticFlight) && !IsOnGround()));

            NodeTransition staticFlightToEmptyAction = new(emptyAction,
                new(() => timer.HasTimerExpired<StaticFlight>()));
            #endregion

            #region Completion
            StateNode.Creator<Nodes.Common.Empty>.Using(emptyMovement).
                WithTransition(emptymovementToIdle).
                Accomplish(controller);

            StateNode.Creator<Idle>.Using(idle).
                WithTransition(toWalk).
                WithTransition(toEmptyMovement).
                WithTransition(toRun).
                WithAnimation("main-idle").
                Accomplish(controller);

            StateNode.Creator<Walk>.Using(walk).
                WithTransition(toIdle).
                WithTransition(toEmptyMovement).
                WithTransition(toRun).
                WithTransition(toDash).
                WithAnimation("main_walk").
                Accomplish(controller);

            StateNode.Creator<Run>.Using(run).
                WithTransition(toWalk).
                WithTransition(toEmptyMovement).
                WithTransition(toIdle).
                WithTransition(toDash).
                WithAnimation("main-run").
                Accomplish(controller);

            StateNode.Creator<Dash>.Using(dash).
                WithTransition(dashToIdle).
                WithAnimation("main-dash").
                WithAction(() => StartTimer<Dash>(0.7f)).
                Accomplish(controller);

            // Action
            StateNode.Creator<Nodes.Common.Empty>.Using(emptyAction).
                WithTransition(toJumpCharging).
                WithTransition(toAttack).
                WithTransition(toOnAir).
                Accomplish(controller);

            StateNode.Creator<JumpCharging>.Using(jumpCharging).
                WithTransition(toJump).
                WithTransition(toOnAir).
                WithAnimation("main-jumpcharging").
                Accomplish(controller);

            StateNode.Creator<Jump>.Using(jump).
                WithTransition(toEmptyAction).
                WithTransition(toDoubleJump).
                WithTransition(toStaticFlight).
                Accomplish(controller);

            StateNode.Creator<DoubleJump>.Using(doubleJump).
                WithTransition(toEmptyAction).
                WithTransition(toStaticFlight).
                Accomplish(controller);

            StateNode.Creator<StaticFlight>.Using(staticFlight).
                WithTransition(staticFlightToEmptyAction).
                WithAnimation("main-holding").
                Accomplish(controller);

            StateNode.Creator<OnAir>.Using(onAir).
                WithTransition(toStaticFlight).
                WithTransition(toEmptyAction).
                WithAnimation("main-hit").
                Accomplish(controller);

            StateNode.Creator<AttackNormal>.Using(attackNormal).
                WithAnimation("main-attack").
                WithTransition(attackToEmpty).
                Accomplish(controller);
            #endregion
        }

        private bool IsStopped()
        {
            return Mathf.Abs(controlData.velocityDirection) == 0;
        }

        public void UpdateInput()
        {
            var velocity = input.Player.Transit.ReadValue<Vector2>();
            controlData.velocityDirection = velocity.x > 0 ? 1 : velocity.x < 0 ? -1 : 0;
        }

        private void OnDestroy()
        {
            inputController.Dispose();
        }
    }
}
