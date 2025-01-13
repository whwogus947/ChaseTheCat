using UnityEngine;

namespace Com2usGameDev
{
    public class CatHairBallSpawner : MapSpawner
    {
        [SerializeField] private AbilityBundleSO abilityBundle;
        [SerializeField] private GachaGradeCollectionsSO collections;

        public override void Spawn(GameObject spawnable)
        {
            foreach (var grade in collections.grades)
                if (grade.TryInstantiateByGrade(abilityBundle.grade, out CatHairBall hairBall))
                    Initialize(hairBall);
        }

        private void Initialize(CatHairBall hairBall)
        {
            hairBall.SetAbility(abilityBundle);
            hairBall.transform.position = transform.position;
        }
    }
}