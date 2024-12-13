using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SocialInteraction : MonoBehaviour
    {
        public NPCTypeSO playerType;
        public Button button;
        public TMP_Text player;
        public TMP_Text npc;

        private Dialogue dialogue;

        public void NextMessage()
        {
            TMP_Text targetText = dialogue.NPC == playerType ? player : npc;
            dialogue.NextMessage(targetText, true);
        }

        public void Open(Dialogue dialogue)
        {
            Time.timeScale = 0f;

            this.dialogue = dialogue;
            transform.GetChild(0).gameObject.SetActive(true);
            dialogue.StartNewMessage();
            dialogue.onLastDialogueEnd += () => {transform.GetChild(0).gameObject.SetActive(false); Time.timeScale = 1f;};
            button.onClick.AddListener(NextMessage);
            NextMessage();
        }
    }
}
