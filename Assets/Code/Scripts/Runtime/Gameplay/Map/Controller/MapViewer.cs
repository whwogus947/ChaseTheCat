using Cysharp.Threading.Tasks;
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

        public SceneReference testScene;

        private UnityAction<StageData> onSelectStage;
        private UIDocument document;
        private VisualElement root;
        private VisualElement backBoard;
        private Vector2 intervalDistance = new(200, 100);
        private Vector2 Center => new(root.resolvedStyle.width / 2f, root.resolvedStyle.height / 2f);

        void Start()
        {
            // await UniTask.WaitForSeconds(0.2f);
            onSelectStage = delegate { };

            Debug.Log(backBoard);
            Debug.Log(backBoard.resolvedStyle);
            Debug.Log(backBoard.contentRect.width);
            Debug.Log(backBoard.layout.width);
            Debug.Log(backBoard.style.backgroundSize.value.x);
            Debug.Log(backBoard.worldBound.width);
            Debug.Log(backBoard.worldBound.size.x);
            Debug.Log(backBoard.worldBound.x);
            var center = new Vector2(root.resolvedStyle.width / 2f, root.resolvedStyle.height / 2f);

            var test = new StageData();
            test.scene = testScene;
            test.location = (1, 5);

            SpawnStageButton(test, center);
        }

        private void OnEnable() {
            document = GetComponent<UIDocument>();

            root = document.rootVisualElement;
            backBoard = root.Query<VisualElement>("back-board");
        }

        public void AddOnClickButton(UnityAction<StageData> stageSelectEvt)
        {
            onSelectStage += stageSelectEvt;
        }

        private void SpawnStageButton(StageData data, Vector2 center)
        {
            var clone = buttonAsset.Instantiate();
            clone.style.left = center.x + data.location.room * intervalDistance.x;
            clone.style.top = center.y * 2 - data.location.level * intervalDistance.y;
            // Debug.Log(clone.style.left);
            // Debug.Log(clone.style.top);

            var buttonClone = clone.Q<Button>("select-stage");
            buttonClone.RegisterCallback<ClickEvent>((evt) =>
            {
                onSelectStage(data); 
                evt.StopPropagation();
            });

            backBoard.Add(clone);
        }
    }
}
