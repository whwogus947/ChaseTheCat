using UnityEngine;

namespace Com2usGameDev
{
    public class MapTester : MonoBehaviour
    {
        public RegionDataSO regionData;
        
        void Start()
        {
            InvokeRepeating(nameof(AutoRespawn), 1f, 1f);
        }

        public void CreateTestMapModule()
        {
            var sections = regionData.CreateCompleteSections();
            foreach (var section in sections.Values)
            {
                Instantiate(section.tileMap, transform);
            }
        }

        private void AutoRespawn()
        {
            var sections = regionData.CreateCompleteSections();
            foreach (var section in sections.Values)
            {
                var clone = Instantiate(section.tileMap, transform);
                Destroy(clone.gameObject, 1f);
            }
        }
    }
}
