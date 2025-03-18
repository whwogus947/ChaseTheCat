using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com2usGameDev
{
    public class UIRaycast : MonoBehaviour
    {
        private static readonly List<RaycastResult> m_RaycastResults = new(10);
        private static InformationImage selected;

        public static (bool hasValue, InformationImage image) HasAnyInPoint(Vector2 screenPosition, LayerMask layer)
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return (false, default);

            PointerEventData pointerData = new(eventSystem)
            {
                position = screenPosition
            };

            m_RaycastResults.Clear();
            eventSystem.RaycastAll(pointerData, m_RaycastResults);
            if (m_RaycastResults.Count > 0 && m_RaycastResults.Any(x => 1 << x.gameObject.layer == layer.value))
            {
                var obj = m_RaycastResults.FirstOrDefault(x => 1 << x.gameObject.layer == layer.value);
                if (selected == null || obj.gameObject != selected.gameObject)
                {
                    selected = obj.gameObject.GetComponent<InformationImage>();
                }
                return (selected != null, selected);
            }
            return (false, default);
        }
    }
}
