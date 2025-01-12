using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Section Site", menuName = "Cum2usGameDev/Map/Section/Site")]
    public class SectionSiteSO : ScriptableObject
    {
        [HideInInspector] public string tilemapPath;
        [HideInInspector] public string dataPath;
        public bool contact;

        [Header("Interlock")]
        public SectionSiteSO upside;
        public SectionSiteSO downside;

        [Header("Interlock")]
        public Tilemap template;
    }
}
