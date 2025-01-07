using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Gacha Grade", menuName = "Cum2usGameDev/Gacha/Grade")]
    public class GachaGradeSO : ScriptableObject
    {
        public GradeTypeSO grade;
        public CatHairBall ballPrefab;

        public bool TryInstantiateByGrade(GradeTypeSO grade, out CatHairBall hairBall)
        {
            hairBall = null;
            if (this.grade == grade)
            {
                hairBall = Instantiate(ballPrefab);
                return true;
            }
            return false;
        }
    }
}
