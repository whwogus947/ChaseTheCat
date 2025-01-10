using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Section Site", menuName = "Cum2usGameDev/Map/Section/Site")]
    public class SectionSiteSO : ScriptableObject
    {
        [HideInInspector] public string folderPath;
    }
}
