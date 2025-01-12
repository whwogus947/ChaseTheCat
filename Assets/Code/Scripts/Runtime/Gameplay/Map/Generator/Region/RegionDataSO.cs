using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Region Data", menuName = "Cum2usGameDev/Map/Region/Data")]
    public class RegionDataSO : ScriptableObject
    {
        [SerializeField] private RegionTypeSO regionType;
        [SerializeField] private NPCTypeSO npc;
        // [SerializeField] private story
        [SerializeField] private List<SectionBundle> sectionBundles;
        [SerializeField] private MapEnemyProvider specialEnemies;

        public Dictionary<SectionSiteSO, SectionDataSO> CreateCompleteSections()
        {
            Dictionary<SectionSiteSO, List<SectionDataSO>> siteSections = new();
            Dictionary<SectionSiteSO, SectionDataSO> result = new();
            foreach (var bundle in sectionBundles)
            {
                if (!siteSections.ContainsKey(bundle.site))
                    siteSections[bundle.site] = new();

                foreach (var section in bundle.sections)
                {
                    siteSections[bundle.site].Add(section);
                }
            }

            foreach (var site in siteSections.Keys)
            {
                if (!site.contact)
                    continue;

                var selectedSection = siteSections[site].GetRandom();
                var selectedSite = selectedSection.site;
                var upsideSite = selectedSite.upside;
                var downsideSite = selectedSite.downside;

                siteSections[upsideSite]?.RemoveAll(x => x.downsideData != selectedSection);
                siteSections[downsideSite]?.RemoveAll(x => x.upsideData != selectedSection);
                result[site] = selectedSection;
            }

            foreach (var site in siteSections.Keys)
            {
                if (site.contact)
                    continue;
                Debug.Log(site);

                var selectedSection = siteSections[site].GetRandom();
                result[site] = selectedSection;
            }

            return result;
        }
    }

    [System.Serializable]
    public class SectionBundle
    {
        public string name;
        public SectionSiteSO site;
        public List<SectionDataSO> sections;
    }
}
