using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Ability Bundle", menuName = "Cum2usGameDev/Ability/Bundle")]
    public class AbilityBundleSO : ScriptableObject
    {
        public GradeTypeSO grade;
        public List<AbilitySO> abilities;
    }
}
