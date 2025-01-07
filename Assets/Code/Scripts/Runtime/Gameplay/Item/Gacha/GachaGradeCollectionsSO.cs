using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Gacha Collection", menuName = "Cum2usGameDev/Gacha/GradeCollection")]
    public class GachaGradeCollectionsSO : ScriptableObject
    {
        public List<GachaGradeSO> grades;
    }
}
