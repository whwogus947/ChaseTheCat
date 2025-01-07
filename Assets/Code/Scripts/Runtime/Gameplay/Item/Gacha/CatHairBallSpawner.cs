using UnityEngine;

namespace Com2usGameDev
{
    public class CatHairBallSpawner : MonoBehaviour
    {
        public bool onAwake;
        public bool atPoint;
        [SerializeField] private AbilityBundleSO abilityBundle;
        [SerializeField] private GachaGradeCollectionsSO collections;

        void Start()
        {
            if (onAwake)
                Spawn();
        }

        private void Spawn()
        {
            foreach (var grade in collections.grades)
                if (grade.TryInstantiateByGrade(abilityBundle.grade, out CatHairBall hairBall))
                    Initialize(hairBall);
        }

        private void Initialize(CatHairBall hairBall)
        {
            hairBall.SetAbility(abilityBundle);
            hairBall.transform.position = SpawnPoint();
        }

        private Vector2 SpawnPoint() => atPoint ? transform.position : Vector2.zero;
    }
}