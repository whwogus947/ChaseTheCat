using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class FlotSaveSlot : MonoBehaviour
    {
        public TMP_Text title;
        public TMP_Text caption;
        public Button save;

        public void SetData(string titleText, string captionText, UnityAction onLoadEvent)
        {
            title.text = titleText;
            caption.text = captionText;
            save.onClick.RemoveAllListeners();
            save.onClick.AddListener(onLoadEvent);
        }
    }
}
