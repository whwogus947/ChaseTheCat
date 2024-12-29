using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Void Handler", menuName = "Cum2usGameDev/Core/EventHandler/Void")]
    public class VoidChannelSO : ScriptableObject
    {
        private UnityAction @event;

        public void Invoke() => @event?.Invoke();

        public void AddEvent(UnityAction evt) => @event += evt;

        public void RemoveEvent(UnityAction evt) => @event -= evt;

        public void UniqueEvent(UnityAction evt) => @event = evt;
    }
}
