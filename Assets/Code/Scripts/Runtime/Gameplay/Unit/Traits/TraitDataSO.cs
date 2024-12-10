using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Trait Data", menuName = "Cum2usGameDev/Trait/TraitType")]
    public class TraitDataSO : ScriptableObject
    {
        public GradeTypeSO grade;
    }
}
