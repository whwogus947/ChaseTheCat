using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class NPCBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private Dialogue dialogue;
        private SocialInteraction social;
        private UnityAction onInteract;
        private SinewaveTranslator questionMark;

        void Start()
        {
            social = FindAnyObjectByType<SocialInteraction>();
            questionMark = GetComponentInChildren<SinewaveTranslator>();
        }

        public void OpenDialogue()
        {
            Destroy(questionMark.gameObject);
            onInteract?.Invoke();
            social.Open(dialogue);
        }

        public void AddEvent(UnityAction @event)
        {
            onInteract += @event;
        }

        public void Interact()
        {
            OpenDialogue();
        }
    }
}
