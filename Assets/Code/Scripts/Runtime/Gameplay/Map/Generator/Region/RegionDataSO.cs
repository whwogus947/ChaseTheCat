using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Region Data", menuName = "Cum2usGameDev/Map/Region/Data")]
    public class RegionDataSO : ScriptableObject
    {
        [SerializeField] private RegionTypeSO regionType;
        [SerializeField] private NPCTypeSO npc;
        // [SerializeField] private story
        [SerializeField] private SectionDataSO sectionData;
        [SerializeField] private MapEnemyProvider specialEnemies;
        [SerializeField] private RegionButtonDataSO buttonData;
    }
}
