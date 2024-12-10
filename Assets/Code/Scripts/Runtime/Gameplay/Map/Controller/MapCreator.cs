using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace Com2usGameDev
{
    public class MapCreator : MonoBehaviour
    {
        public List<MapStage> stages;
        public int levelCount;
        public Vector2Int minMaxPerRoom;

        private StageData rootStage;

        void Awake()
        {
            rootStage = new StageData(RandomStage().scene.Name, (0, 0), (0, 200));
            CreateMap(rootStage);
            AssignPositions(rootStage);
        }

        public StageData GetRootStage() => rootStage;

        private void CreateMap(StageData parentNode)
        {
            if (parentNode.Location.level >= levelCount)
                return;

            int level = parentNode.Location.level + 1;
            int roomCount = (minMaxPerRoom.x, minMaxPerRoom.y).RandomValue();
            for (int room = 0; room < roomCount; room++)
            {
                StageData child = new(RandomStage().scene.Name, (room, level));
                parentNode.Add(child);
                CreateMap(child);
            }
        }

        private void AssignPositions(StageData parentNode)
        {
            int childCount = parentNode.ChildCount;
            if (childCount < 1)
                return;

            float intervalX = 200 + (60 * (levelCount - parentNode.Location.level));

            float offsetX = parentNode.Position.x + -intervalX * (childCount - 1) * 0.5f;
            float y = parentNode.Position.y + 100;
            Debug.Log(parentNode.Position);
            for (int i = 0; i < childCount; i++)
            {
                var child = parentNode.GetChild(i);
                child.Position = new(offsetX + intervalX * i, y);
                AssignPositions(child);
            }
        }

        private MapStage RandomStage() => stages[Random.Range(0, stages.Count)];
    }

    [System.Serializable]
    public class MapStage
    {
        public SceneReference scene;
    }

    public class StageData
    {
        public string SceneName {get; private set;}
        public (int room, int level) Location {get; private set;}
        public (float x, float y) Position { get; set; }
        public int ChildCount => childs.Count;

        private readonly List<StageData> childs;

        public StageData(string sceneName, (int, int) location, (float, float) position = default)
        {
            this.SceneName = sceneName;
            this.Location = location;
            this.Position = position;
            childs = new();
        }

        public StageData GetChild(int index) => childs[index];

        public void Add(StageData child) => childs.Add(child);
    }
}
