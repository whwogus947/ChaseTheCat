using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com2usGameDev
{
    public class UIRaycast : MonoBehaviour
    {
        private static readonly List<RaycastResult> m_RaycastResults = new(10);

        public static bool HasAnyInPoint(Vector2 screenPosition)
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return false;

            PointerEventData pointerData = new(eventSystem)
            {
                position = screenPosition
            };

            m_RaycastResults.Clear();
            eventSystem.RaycastAll(pointerData, m_RaycastResults);
            return m_RaycastResults.Count > 0;
        }
    }
}
