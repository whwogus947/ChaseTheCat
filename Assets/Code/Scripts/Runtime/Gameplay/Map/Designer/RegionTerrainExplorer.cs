using UnityEngine;

namespace Com2usGameDev
{
    public class RegionTerrainExplorer : MonoBehaviour
    {
        [HideInInspector] public SectionSiteSO section;
        [HideInInspector] public int currentIndex = 0;

        private const string SECTION_SHOWCASE = "Section Showcase";

        public Transform Showcase(string siteName)
        {
            var storage = ShowcaseStorage();
            var site = CreateOrGet(storage, siteName);
            return site;
        }

        private Transform ShowcaseStorage()
        {
            var showcase = CreateOrGet(transform, SECTION_SHOWCASE);
            return showcase;
        }

        private Transform CreateOrGet(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child == null)
            {
                var clone = new GameObject(name);
                clone.transform.SetParent(parent);
                child = clone.transform;
            }
            return child;
        }
    }
}
