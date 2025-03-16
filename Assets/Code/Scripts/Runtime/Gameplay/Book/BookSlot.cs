using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class BookSlot : MonoBehaviour
    {
        public Image Icon { get; private set; }

        void Awake()
        {
            Icon = transform.GetChild(0).GetComponent<Image>();
        }
    }
}
