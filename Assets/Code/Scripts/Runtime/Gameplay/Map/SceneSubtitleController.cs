using UnityEngine;
using UnityEngine.UIElements;

namespace Com2usGameDev
{
    public class SceneSubtitleController : MonoBehaviour
    {
        public UIDocument document;
        public Dialogue dialogue;
        public string labelName;
        public string nextButtonName;
        public SceneHandler scene;

        void Start()
        {
            PrintLog();
        }
        
        public void PrintLog()
        {
            var root = document.rootVisualElement;
            var label = root.Q<Label>(labelName);
            dialogue.NextMessage(label);
            var button = root.Q<Button>(nextButtonName);
            button.RegisterCallback<ClickEvent>((evt) => dialogue.NextMessage(label, true));
            dialogue.onLastDialogueEnd += scene.LoadScene;
        }
    }
}
