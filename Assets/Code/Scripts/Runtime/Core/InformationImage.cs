using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    [RequireComponent(typeof(Image))]
    public class InformationImage : MonoBehaviour
    {
        public IDescription descriptionItem;

        private Image image;

        public Image GetImage()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            return image;
        }
    }
}
