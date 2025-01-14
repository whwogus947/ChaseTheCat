using UnityEngine;

namespace Com2usGameDev
{
    public class CatHairBallSpawner : MapSpawner
    {
        [SerializeField] private AbilityBundleSO abilityBundle;
        [SerializeField] private GachaGradeCollectionsSO collections;

        public override GameObject Spawn(GameObject spawnable)
        {
            var ball = spawnable.GetComponent<CatHairBall>();
            ball.transform.position = transform.position;
            return ball.gameObject;
        }
    }

    [System.Serializable]
    public class CatBallLink : ISpawnable
    {
        [SerializeField] private AbilityBundleSO abilityBundle;
        private GachaGradeCollectionsSO gradeData;

        public GameObject Spawnable
        {
            get
            {
                foreach (var grade in gradeData.grades)
                    if (grade.TryInstantiateByGrade(abilityBundle.grade, out CatHairBall hairBall))
                    {
                        Initialize(hairBall);
                        return hairBall.gameObject;
                    }

                return null;
            }
        }

        public void SetInfo(GachaGradeCollectionsSO gradeData)
        {
            this.gradeData = gradeData;
        }

        private void Initialize(CatHairBall hairBall)
        {
            hairBall.SetAbility(abilityBundle);
        }
    }
}