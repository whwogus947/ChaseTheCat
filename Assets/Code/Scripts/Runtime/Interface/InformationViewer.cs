using TMPro;
using UnityEngine;

namespace Com2usGameDev
{
    public class InformationViewer : MonoBehaviour
    {
        public LayerMask InfoUILayer;
        public RectTransform target;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private float spacing = 50;

        private readonly Vector2 screenSize = new(1920, 1080);

        void Start()
        {

        }

        void Update()
        {
            var pos = Input.mousePosition;
            var result = UIRaycast.HasAnyInPoint(pos, InfoUILayer);
            target.gameObject.SetActive(result.hasValue);
            if (result.hasValue)
            {
                Deploy(pos, new(target.rect.width / 2f, target.rect.height / 2f));
                descriptionText.SetText(result.image.descriptionItem.GetDescription());
            }
        }

        public void Deploy(Vector2 pos, Vector2 size)
        {
            float offsetX = pos.x + size.x > screenSize.x - spacing ? -(pos.x + size.x - screenSize.x + spacing) : pos.x - size.x < spacing ? -(pos.x - size.x - spacing) : 0;
            float offsetY = pos.y + size.y > screenSize.y - spacing ? -(pos.y + size.y - screenSize.y + spacing) : pos.y - size.y < spacing ? -(pos.y - size.y - spacing) : 0;
            target.position = pos + new Vector2(offsetX, offsetY);
        }
    }
}
