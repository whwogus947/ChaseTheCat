using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Section Data", menuName = "Cum2usGameDev/Map/Section/Data")]
    public class SectionDataSO : ScriptableObject
    {
        public Tilemap tileMap;
        public SectionSiteSO site;
        [HideInInspector] public SectionDataSO upsideData;
        [HideInInspector] public SectionDataSO downsideData;
    }
}
