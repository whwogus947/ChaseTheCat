using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SocialInteraction : MonoBehaviour
    {
        public NPCTypeSO playerType;
        public Button button;
        public Transform playerUI;
        public TMP_Text player;
        public Transform npcUI;
        public TMP_Text npc;

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

        public void Open(Dialogue dialogue)
        {
            Time.timeScale = 0f;
            button.onClick.RemoveAllListeners();
            dialogue.onLastDialogueEnd = delegate {};
            npc.text = "";
            player.text = "";

            this.dialogue = dialogue;
            transform.GetChild(0).gameObject.SetActive(true);
            dialogue.StartNewMessage();
            dialogue.onLastDialogueEnd += () => {transform.GetChild(0).gameObject.SetActive(false); Time.timeScale = 1f;};
            button.onClick.AddListener(NextMessage);
            prevNPC = dialogue.NPC;
            NextMessage();
        }

        private TMP_Text GetCurrentText() => dialogue.NPC == playerType ? player : npc;
        
        private void SelectPanel()
        {
            if (dialogue.NPC == playerType)
            {   
                npcUI.SetSiblingIndex(0);
                playerUI.SetSiblingIndex(2);
            }
            else
            {
                playerUI.SetSiblingIndex(0);
                npcUI.SetSiblingIndex(2);
            }
        }
    }
}
