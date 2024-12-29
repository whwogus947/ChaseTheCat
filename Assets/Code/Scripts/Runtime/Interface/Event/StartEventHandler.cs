using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class StartEventHandler : MonoBehaviour
    {
        public UnityEvent @event;
        
        void Start()
        {
            @event.Invoke();
        }
    }
}
