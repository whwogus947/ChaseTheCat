using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Region Data", menuName = "Cum2usGameDev/Map/Region/Data")]
    public class RegionDataSO : ScriptableObject
    {
        [SerializeField] private RegionTypeSO regionType;
        // [SerializeField] private story
        [SerializeField] private List<SectionBundle> sectionBundles;

        [Header("NPC")]
        [SerializeField] private NPCBehaviour npc;

        [Header("MONSTERS")]
        [SerializeField] private MapEnemyProvider specialEnemies;

        [Header("CAT BALL")]
        [SerializeField] private GachaGradeCollectionsSO gradeData;
        [SerializeField] private List<CatBallLink> catballs;

        [Header("PASSAGE")][SerializeField] private MapPassage passage;

        public void GenerateRegion(Transform storage)
        {
            var sections = CreateCompleteSections();
            TerrainCreation(sections, storage);

            SpawnPlayer(sections);
            SpawnEnemy(sections);
            SpawnNPC(sections);
            SpawnCatBall(sections);
            SpawnPassage(sections);
        }

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

                var selectedSection = siteSections[site].GetRandom();
                result[site] = selectedSection;
            }

            return result;
        }

        private List<GameObject> Spawn<T1, T2>(Dictionary<SectionSiteSO, SectionDataSO> sections, List<T2> targets, int capacity = 1) where T1 : MapSpawner where T2 : ISpawnable
        {
            var spawners = SpawnPoints<T1>(sections, capacity);

            int spawnCount = Mathf.Min(capacity, spawners.Count);
            List<GameObject> spawned = new();
            for (int i = 0; i < spawnCount; i++)
            {
                var spawner = spawners.GetRandom();
                spawners.Remove(spawner);
                var temp = spawner.Spawn(targets.GetRandom().Spawnable);
                spawned.Add(temp);
            }
            return spawned;
        }

        private GameObject Spawn<T1, T2>(Dictionary<SectionSiteSO, SectionDataSO> sections, T2 target, int capacity = 1, bool isSole = true) where T1 : MapSpawner where T2 : ISpawnable
        {
            var spawners = SpawnPoints<T1>(sections, capacity);

            var spawner = spawners.GetRandom();
            return spawner.Spawn(target.Spawnable);
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

        private void TerrainCreation(Dictionary<SectionSiteSO, SectionDataSO> sections, Transform storage)
        {
            foreach (var section in sections.Values)
            {
                Instantiate(section.tileMap, storage);
            }
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
            Spawn<PlayerSpawner, PlayerBehaviour>(sections, null, isSole: true);
        }

        private void SpawnCatBall(Dictionary<SectionSiteSO, SectionDataSO> sections)
        {
            catballs.ForEach(catball => catball.SetInfo(gradeData));
            var ball = Spawn<CatHairBallSpawner, CatBallLink>(sections, catballs);
        }

        private void SpawnPassage(Dictionary<SectionSiteSO, SectionDataSO> sections)
        {
            Spawn<PassageSpawner, MapPassage>(sections, passage);
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
