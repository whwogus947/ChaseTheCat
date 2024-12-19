using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class NPCBehaviour : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;
        private SocialInteraction social;
        private UnityAction onInteract;

        void Start()
        {
            social = FindAnyObjectByType<SocialInteraction>();
        }

        public void OpenDialogue()
        {
            onInteract?.Invoke();
            social.Open(dialogue);
        }

        public void AddEvent(UnityAction @event)
        {
            onInteract += @event;
        }
    }
}
