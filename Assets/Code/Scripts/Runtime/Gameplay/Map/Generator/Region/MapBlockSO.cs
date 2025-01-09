using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Region Block", menuName = "Cum2usGameDev/Map/Block/Data")]
    public class MapBlockSO : ScriptableObject
    {
        public Tilemap tileMap;
        [HideInInspector] public BlockLocation location;
        [HideInInspector] public BlockMatchTypeSO upside;
        [HideInInspector] public BlockMatchTypeSO downside;
    }

    public enum BlockLocation
    {
        Bottom,
        Middle,
        Top,
    }
}
