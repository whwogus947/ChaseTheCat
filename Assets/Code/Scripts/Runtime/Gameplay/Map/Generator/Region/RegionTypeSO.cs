using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Region Type", menuName = "Cum2usGameDev/Map/Region/Type")]
    public class RegionTypeSO : ScriptableObject
    {
        [SerializeField] private RegionButtonDataSO buttonData;
    }
}
