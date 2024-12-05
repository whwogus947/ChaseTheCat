using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Com2usGameDev
{
    [RequireComponent(typeof(UIDocument))]
    public class RuntimeButton : MonoBehaviour
    {
        public string buttonName;
        [SerializeField] private UnityEvent @event;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var button = root.Q<Button>(buttonName);
            button.RegisterCallback<ClickEvent>((evt) => @event.Invoke());
        }
    }
}
