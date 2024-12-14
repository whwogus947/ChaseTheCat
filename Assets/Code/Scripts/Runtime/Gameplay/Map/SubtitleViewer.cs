using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SubtitleViewer : MonoBehaviour
    {
        public SceneHandler loadScene;
        public Dialogue dialogue;

        private TMP_Text viewText;
        private Button button;

        void Start()
        {
            button = GetComponentInChildren<Button>();
            viewText = GetComponentInChildren<TMP_Text>();

            Initialize();            
        }

        private void Initialize()
        {
            dialogue.onLastDialogueEnd += loadScene.LoadScene;
            button.onClick.AddListener(NextMessage);
            NextMessage();
        }

        private void NextMessage()
        {
            dialogue.NextMessage(viewText, true);
        }
    }
}
