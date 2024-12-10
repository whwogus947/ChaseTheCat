using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Com2usGameDev
{
    [RequireComponent(typeof(UIDocument))]
    public class MapViewer : MonoBehaviour
    {
        public VisualTreeAsset buttonAsset;

        private UnityAction<StageData> onSelectStage;
        private VisualElement backBoard;
        private VisualElement lineBoard;
        private Vector2 Center => new(canvasCenter.x / 2f, canvasCenter.y / 2f);
        private Vector2 canvasCenter = new(1920, 1080);
        private (float x, float y) buttonOffset = (30, 30);

        private void Awake()
        {
            onSelectStage = delegate { };
        }

        void OnEnable()
        {
            var document = GetComponent<UIDocument>();
            var root = document.rootVisualElement;
            root.style.width = canvasCenter.x;
            root.style.height = canvasCenter.y;
            backBoard = root.Q<VisualElement>("back-board");
            lineBoard = root.Q<VisualElement>("line-renderer");

            var exit = root.Q<Button>("exit");
            exit.RegisterCallback<ClickEvent>((evt) => gameObject.SetActive(false));
        }

        public void AddOnClickButton(UnityAction<StageData> stageSelectEvt)
        {
            onSelectStage += stageSelectEvt;
        }

        public void DrawAllStages(StageData root)
        {
            DrawStageButton(root);
        }

        private void DrawStageButton(StageData data)
        {
            var clone = buttonAsset.Instantiate();
            var x = Center.x + data.Position.x;
            var y = Center.y * 2 - data.Position.y;
            clone.style.left = x;
            clone.style.top = y;

            var buttonClone = clone.Q<Button>("select-stage");
            buttonClone.RegisterCallback<ClickEvent>((evt) =>
            {
                onSelectStage(data);
                evt.StopPropagation();
            });
            clone.style.position = Position.Absolute;

            backBoard.Add(clone);

            for (int i = 0; i < data.ChildCount; i++)
            {
                var child = data.GetChild(i);
                DrawStageButton(child);
                lineBoard.DrawLine((x + buttonOffset.x, y + buttonOffset.y), (Center.x + child.Position.x + buttonOffset.x, Center.y * 2 - child.Position.y + buttonOffset.y));
            }
        }
    }
}
