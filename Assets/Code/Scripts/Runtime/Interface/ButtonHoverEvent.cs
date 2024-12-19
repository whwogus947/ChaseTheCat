using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class ButtonHoverEvent : MonoBehaviour
    {
        private Image[] images;

        void Awake()
        {
            images = GetComponentsInChildren<Image>();
            Off();
        }
    
        public void On()
        {
            foreach (var item in images)
            {
                item.color = Color.white;
            }
        }

        public void Off()
        {
            foreach (var item in images)
            {
                item.color = Color.gray;
            }
        }
    }
}
