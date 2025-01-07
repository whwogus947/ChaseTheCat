using UnityEngine;

namespace Com2usGameDev
{
    public static class GameObjectExtensions
    {
        public static T GetComponentInEntire<T>(this GameObject obj) where T : MonoBehaviour
        {
            var component = obj.GetComponentInChildren<T>();
            if (component != null)
                return component;
                
            component = obj.GetComponentInParent<T>();
            if (component == null)
                Debug.LogError($"can not find {typeof(T)}");

            return component;
        }
    }
}
