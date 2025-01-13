using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Region Data", menuName = "Cum2usGameDev/Map/Region/Data")]
    public class RegionDataSO : ScriptableObject
    {
        [SerializeField] private RegionTypeSO regionType;
        [SerializeField] private NPCBehaviour npc;
        // [SerializeField] private story
        [SerializeField] private List<SectionBundle> sectionBundles;
        [SerializeField] private MapEnemyProvider specialEnemies;
        PlayerBehaviour player;

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

        private void Spawn<T1, T2>(Dictionary<SectionSiteSO, SectionDataSO> sections, List<T2> targets, int capacity = 1) where T1 : MapSpawner where T2 : ISpawnable
        {
            var spawners = SpawnPoints<T1>(sections, capacity);

            int spawnCount = Mathf.Min(capacity, spawners.Count);
            for (int i = 0; i < spawnCount; i++)
            {
                var spawner = spawners.GetRandom();
                spawners.Remove(spawner);
                targets.GetRandom().Spawn(spawner);
            }
        }

        private void Spawn<T1, T2>(Dictionary<SectionSiteSO, SectionDataSO> sections, T2 target, int capacity = 1) where T1 : MapSpawner where T2 : ISpawnable
        {
            var spawners = SpawnPoints<T1>(sections, capacity);

            var spawner = spawners.GetRandom();
            target.Spawn(spawner);
        }

        private List<T> SpawnPoints<T>(Dictionary<SectionSiteSO, SectionDataSO> sections, int capacity) where T : MapSpawner 
        {
            List<T> spawners = new(capacity);
            foreach (var section in sections)
            {
                var temp = section.Value.tileMap.GetComponentsInChildren<T>();
                spawners.AddRange(temp);
            }
            return spawners;
        }

        private void SpawnEnemy(Dictionary<SectionSiteSO, SectionDataSO> sections)
        {
            Spawn<EnemySpawner, MonsterBehaviour>(sections, specialEnemies.monsters);
        }

        private void SpawnNPC(Dictionary<SectionSiteSO, SectionDataSO> sections)
        {
            Spawn<NPCSpawner, NPCBehaviour>(sections, npc);
        }

        private void SpawnPlayer(Dictionary<SectionSiteSO, SectionDataSO> sections)
        {
            Spawn<PlayerSpawner, PlayerBehaviour>(sections, player);
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
