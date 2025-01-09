using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    public class RegionTerrainMaker : MonoBehaviour
    {
        public string assetCreationPath;
        public string prefabName;
        public Tilemap sample;

        public GameObject SampleClone {get; private set;}

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SaveTempClone(Tilemap clone) => SampleClone = clone.gameObject;

        public void ResetClone()
        {
            DestroyImmediate(SampleClone);
            SampleClone = null;
        }
    }
}
