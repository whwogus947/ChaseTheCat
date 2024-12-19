using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SubtitleViewer : MonoBehaviour
    {
        public UnityEvent loadSceneEvent;
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
            dialogue.onLastDialogueEnd += loadSceneEvent.Invoke;
            button.onClick.AddListener(NextMessage);
            NextMessage();
        }

        private void NextMessage()
        {
            dialogue.NextMessage(viewText, true);
        }
    }
}
