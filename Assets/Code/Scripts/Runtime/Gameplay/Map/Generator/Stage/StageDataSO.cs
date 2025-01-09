using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Stage Data", menuName = "Cum2usGameDev/Map/Stage/Data")]
    public class StageDataSO : ScriptableObject
    {
        [SerializeField] private RegionDataSO regionData;
        [SerializeField] private StageTypeSO stageType;
        [SerializeField] private MapEnemyProvider sharedEnemies;
    }
}
