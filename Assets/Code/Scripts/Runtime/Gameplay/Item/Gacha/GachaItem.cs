using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class GachaItem : MonoBehaviour
    {
        public Image gachaCase;
        public Image gachaIcon;
        
        public void SetGachaItem(Sprite gachaCase, Sprite gachaIcon)
        {
            this.gachaCase.sprite = gachaCase;
            this.gachaIcon.sprite = gachaIcon;
            this.gachaIcon.SetNativeSize();
        }
    }
}
