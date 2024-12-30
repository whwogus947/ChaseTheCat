using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class MapNamePopup : MonoBehaviour
    {
        public TMP_Text mapName;
        public Image mapIcon;

        private CanvasGroup group;
        
        void Start()
        {
            group = GetComponentInChildren<CanvasGroup>();
            Blink().Forget();
        }

        public void Setup(string name, Sprite icon)
        {
            if (name == "" || icon == null)
                return;
                
            mapName.text = name;
            mapIcon.sprite = icon;
        }

        private async UniTaskVoid Blink()
        {
            await Tween.Alpha(group, 0.5f, 1f, 1f);
            await UniTask.WaitForSeconds(1f);
            await Tween.Alpha(group, 1f, 0f, 1f);
            Destroy(gameObject);
        }
    }
}
