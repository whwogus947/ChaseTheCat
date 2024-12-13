using UnityEngine;

namespace Com2usGameDev
{
    public class NPCBehaviour : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;
        private SocialInteraction social;

        void Start()
        {
            social = FindAnyObjectByType<SocialInteraction>();
        }

        public void OpenDialogue()
        {
            social.Open(dialogue);
        }
    }
}
