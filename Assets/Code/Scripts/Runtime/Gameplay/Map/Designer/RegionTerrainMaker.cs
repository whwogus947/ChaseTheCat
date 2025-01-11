using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    public class RegionTerrainMaker : MonoBehaviour
    {
        [Header("Workshop")]
        [HideInInspector] public SectionSiteSO section;
        public GameObject TemplateClone {get; private set;}

        private const string SECTION_PALETTE = "Section Palette";

        public void SaveTempClone(Tilemap clone) => TemplateClone = clone.gameObject;

        public void ResetClone()
        {
            DestroyImmediate(TemplateClone);
            TemplateClone = null;
        }

        public void ClearAll()
        {
            var storage = FindSectionPalette();
            for (int i = storage.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(storage.GetChild(i).gameObject);
            }
            TemplateClone = null;
        }

        public Transform FindSectionPalette()
        {
            var palette = transform.Find(SECTION_PALETTE);
            if (palette == null)
            {
                var clone = new GameObject(SECTION_PALETTE);
                clone.transform.SetParent(transform);
                palette = clone.transform;
            }
            return palette;
        }
    }
}
