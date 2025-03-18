using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SalesItem : MonoBehaviour
    {
        public ISalesItem item;
        public Image profile;
        public TMP_Text nameText;
        public TMP_Text priceText;
        
        void Start()
        {
        
        }

        public void SetItem(Sprite _profile, string _nameText, int _price)
        {
            profile.sprite = _profile;
            nameText.SetText(_nameText);
            priceText.SetText("{0}", _price);
        }
    }
}
