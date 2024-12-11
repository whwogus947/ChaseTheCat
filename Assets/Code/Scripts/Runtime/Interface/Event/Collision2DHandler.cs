using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class Collision2DHandler : MonoBehaviour
    {
        public UnityEvent @event;

        private void OnTriggerEnter2D(Collider2D other)
        {
            @event.Invoke();
        }
    }
}
