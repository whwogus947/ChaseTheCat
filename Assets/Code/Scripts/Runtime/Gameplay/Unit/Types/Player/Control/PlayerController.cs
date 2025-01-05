using UnityEngine;

namespace Com2usGameDev
{
    [RequireComponent(typeof(UserInputHandler))]
    public class PlayerController : UnitControl
    {
        private StateController controller;
        private PlayerBehaviour behaviour;
        private UserInputHandler userInput;

        private void Awake()
        {
            behaviour = gameObject.GetComponentInEntire<PlayerBehaviour>();
            userInput = gameObject.GetComponent<UserInputHandler>();
        }

        void Start()
        {
            controller = new StateController(behaviour);
            userInput.BindInputToController(controller, behaviour.controlData, behaviour.abilityController);
        }
        
        void Update()
        {
            userInput.UpdateInput();
            controller.Run();
        }
    }
}
