using UnityEngine;

namespace Com2usGameDev
{
    [RequireComponent(typeof(PlayerBehaviour), typeof(UserInputHandler))]
    public class PlayerController : MonoBehaviour
    {
        private StateController controller;
        private PlayerBehaviour behaviour;
        private UserInputHandler userInput;

        private void Awake()
        {
            behaviour = GetComponent<PlayerBehaviour>();
            userInput = GetComponent<UserInputHandler>();
        }

        void Start()
        {
            controller = new StateController(behaviour);
            userInput.BindInputToController(controller);
        }
        
        void Update()
        {
            userInput.UpdateInput();
            controller.Run();
        }
    }
}
