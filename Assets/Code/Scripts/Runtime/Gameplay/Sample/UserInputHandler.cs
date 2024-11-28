using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com2usGameDev.Dev
{
    public class UserInputHandler : MonoBehaviour
    {
        public InputControllerSO inputController;
        public LinearStatSO direction;

        private PCInput input;
        private bool isRunKeyPressed;

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
            input.Player.Run.performed += OnPressRun;
            input.Player.Run.canceled += OnReleaseRun;
        }

        private void OnReleaseRun(InputAction.CallbackContext context)
        {
            isRunKeyPressed = false;
        }

        private void OnPressRun(InputAction.CallbackContext context)
        {
            isRunKeyPressed = true;
        }

        public void BindInputToController(StateController controller)
        {
            var idle = StateNode.Creator<Nodes.Idle>.CreateType(State.Movement).InProgress();
            var walk = StateNode.Creator<Nodes.Walk>.CreateType(State.Movement).InProgress();
            var run = StateNode.Creator<Nodes.Run>.CreateType(State.Movement).InProgress();

            NodeTransition toIdle = new(idle, new(() => Mathf.Abs(direction.value) == 0));
            NodeTransition toWalk = new(walk, new(() => Mathf.Abs(direction.value) > 0 && !isRunKeyPressed));
            NodeTransition toRun = new(run, new(() => Mathf.Abs(direction.value) > 0 && isRunKeyPressed));

            StateNode.Creator<Nodes.Idle>.With(idle).WithTransition(toWalk).WithTransition(toRun).WithAnimation("main-idle").Accomplish(controller);
            StateNode.Creator<Nodes.Walk>.With(walk).WithTransition(toIdle).WithTransition(toRun).WithAnimation("main_walk").Accomplish(controller);
            StateNode.Creator<Nodes.Run>.With(run).WithTransition(toWalk).WithTransition(toIdle).WithAnimation("main-run").Accomplish(controller);
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
