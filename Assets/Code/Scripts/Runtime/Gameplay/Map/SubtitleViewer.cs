using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SubtitleViewer : MonoBehaviour
    {
        public Dialogue dialogue;
        public SceneHandler scene;
        public TMP_Text viewText;
        public Button button;

        void Start()
        {
            dialogue.onLastDialogueEnd += scene.LoadScene;
            button.onClick.AddListener(NextMessage);
            NextMessage();
        }

        public void NextMessage()
        {
            dialogue.NextMessage(viewText, true);
        }
    }
}
