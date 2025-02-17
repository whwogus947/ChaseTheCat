using UnityEngine;

namespace Com2usGameDev
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj, true);
        }
    }

    public interface ISerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
    }
}