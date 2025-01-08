using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class StageButton : MonoBehaviour
    {
        public RegionButtonDataSO data;
        public AdjacentRoute[] routes;
        public bool isStartStage = false;
        public bool isVisited = false;

        private Color visitedColor;
        private Button button;
        private Image icon;

        void Awake()
        {
            if (ColorUtility.TryParseHtmlString("#FFD49E", out Color color))
            {
                visitedColor = color;
            }
            button = GetComponent<Button>();
            icon = GetComponent<Image>();
            SetUnsivited();
            button.onClick.AddListener(Visit);
            button.interactable = false;
        }

        public Button GetButton()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            return button;
        }

        private void Start()
        {
            if (isStartStage)
            {
                OnVisitStage();
            }
        }

        public void SetVisitable()
        {
            icon.sprite = data.visitable;
        }

        public void SetUnsivited()
        {
            icon.sprite = data.unvisited;
        }

        public void SetVisited()
        {
            icon.sprite = data.visited;
        }

        public void Visit()
        {
            data.Load();
            OnVisitStage();
        }

        private void OnVisitStage()
        {
            foreach (var route in routes)
            {
                route.track.color = visitedColor;
                if (route.button.isVisited)
                {
                    route.button.SetVisited();
                }
                else if (!route.button.isVisited)
                {
                    route.button.SetVisitable();
                }
                route.button.GetButton().interactable = true;
            }
            button.interactable = true;
            SetVisited();
            isVisited = true;
        }
    }

    [System.Serializable]
    public class AdjacentRoute
    {
        public StageButton button;
        public Image track;
    }
}
