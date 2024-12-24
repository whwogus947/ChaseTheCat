using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class MapPalette : MonoBehaviour
    {
        public Transform stageStorage;
        public GameObject xMark;

        private StageButton[] buttons;

        private void Start()
        {
            AllocateStages();
            foreach (var button in buttons)
            {
                if (button.isStartStage)
                {
                    GenerateXMark(button.GetButton());
                }
            }
            AddEvent(GenerateXMark);
        }

        public void AddEvent(UnityAction<Button> @event)
        {
            AllocateStages();
            foreach (var button in buttons)
            {
                button.GetButton().onClick.AddListener(() => @event.Invoke(button.GetButton()));
            }
        }

        private void AllocateStages()
        {
            if (buttons == null || buttons.Length == 0)
            {
                buttons = stageStorage.GetComponentsInChildren<StageButton>(true);
            }
        }

        private void GenerateXMark(Button button)
        {
            var clone = Instantiate(xMark, button.transform);
            clone.SetActive(true);
            clone.transform.localPosition = Vector3.zero;
        }
    }
}
