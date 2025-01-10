using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Section Data", menuName = "Cum2usGameDev/Map/Section/Data")]
    public class SectionDataSO : ScriptableObject
    {
        public Tilemap tileMap;
        [HideInInspector] public SectionLocation location;
        [HideInInspector] public SectionInterlock upside;
        [HideInInspector] public SectionInterlock downside;
    }

    public enum SectionLocation
    {
        Top,
        Middle,
        Bottom,
    }
}
