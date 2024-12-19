using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public abstract class EventChannelSO<T> : ScriptableObject
    {
        private UnityAction<T> @event;

        public void Invoke(T type) => @event?.Invoke(type);

        public void AddEvent(UnityAction<T> evt) => @event += evt;

        public void RemoveEvent(UnityAction<T> evt) => @event -= evt;
    }
}
