using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Input Controller", menuName = "Cum2usGameDev/Input/Controller")]
    public class InputControllerSO : ScriptableObject
    {
        public PCInput Input => GetOrCreate();

        private PCInput input;

        public PCInput GetOrCreate()
        {
            input ??= new();
            input.Player.Enable();
            return input;
        }

        public void Dispose()
        {
            input.Player.Disable();
            input = null;
        }
    }
}
