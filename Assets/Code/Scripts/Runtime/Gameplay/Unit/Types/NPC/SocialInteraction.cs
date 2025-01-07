using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SocialInteraction : MonoBehaviour
    {
        [SerializeField] private NPCTypeSO playerType;
        [SerializeField] private Button nextButton;
        [SerializeField] private SocialPage player;
        [SerializeField] private SocialPage npc;
        private Dialogue dialogue;
        private NPCTypeSO prevNPC;

        public void NextMessage()
        {
            SelectPanel();

            if (prevNPC != dialogue.NPC && !dialogue.handler.IsCompleted)
            {
                dialogue.NextMessage(GetCurrentText(), true);
            }
            prevNPC = dialogue.NPC;
            dialogue.NextMessage(GetCurrentText(), true);
        }

        public void Open(Dialogue dialogue, Sprite npcImage)
        {
            npc.portrait.sprite = npcImage;
            Time.timeScale = 0f;
            nextButton.onClick.RemoveAllListeners();
            dialogue.onLastDialogueEnd = delegate {};
            npc.pageMessage.text = "";
            player.pageMessage.text = "";

            this.dialogue = dialogue;
            transform.GetChild(0).gameObject.SetActive(true);
            dialogue.StartNewMessage();
            dialogue.onLastDialogueEnd += () => {transform.GetChild(0).gameObject.SetActive(false); Time.timeScale = 1f;};
            nextButton.onClick.AddListener(NextMessage);
            prevNPC = dialogue.NPC;
            NextMessage();
        }

        private TMP_Text GetCurrentText() => dialogue.NPC == playerType ? player.pageMessage : npc.pageMessage;
        
        private void SelectPanel()
        {
            if (dialogue.NPC == playerType)
            {   
                npc.storage.SetSiblingIndex(0);
                player.storage.SetSiblingIndex(2);
            }
            else
            {
                player.storage.SetSiblingIndex(0);
                npc.storage.SetSiblingIndex(2);
            }
        }
    }

    [System.Serializable]
    public class SocialPage
    {
        public Transform storage;
        public TMP_Text pageMessage;
        public Image portrait;
    }
}
