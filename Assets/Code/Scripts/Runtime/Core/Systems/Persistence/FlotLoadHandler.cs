using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class FlotLoadHandler : MonoBehaviour
    {
        public TMP_Text title;
        public TMP_Text caption;
        public Button load;
        
        public void SetData(string titleText, string captionText, UnityAction onLoadEvent)
        {
            title.text = titleText;
            caption.text = captionText;
            load.onClick.RemoveAllListeners();
            load.onClick.AddListener(onLoadEvent);
        }
    }
}
