using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class NPCBehaviour : MonoBehaviour, IInteractable, ISpawnable
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private SocialInteraction social;
        [SerializeField] private Sprite portraitImage;
        private UnityAction onInteract;
        private SinewaveTranslator questionMark;

        void Start()
        {
            social = Instantiate(social, transform);
            questionMark = GetComponentInChildren<SinewaveTranslator>();
        }

        public void OpenDialogue()
        {
            Destroy(questionMark.gameObject);
            onInteract?.Invoke();
            social.Open(dialogue, portraitImage);
        }

        public void AddEvent(UnityAction @event)
        {
            onInteract += @event;
        }

        public void Interact()
        {
            OpenDialogue();
        }

        public void Spawn(MapSpawner spawner)
        {
            Instantiate(gameObject, spawner.transform.position, Quaternion.identity);
        }
    }
}
