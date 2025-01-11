using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    [CustomEditor(typeof(SectionDataSO))]
    public class SectionDataSOEditor : Editor
    {
        private SectionDataSO data;
        private bool[,] bodyGrid;
        private readonly Color fillColor = Color.black;
        private readonly Color emptyColor = Color.white;
        private readonly Color interlockColor = Color.red;

        private const float MIN_CELL_SIZE = 4;
        private const float MAX_CELL_SIZE = 50f;
        private const float PADDING = 40f;

        private void OnEnable()
        {
            data = (SectionDataSO)target;
            bodyGrid = ConvertTilemapToBoolArray(data.tileMap);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Interlocks", EditorStyles.boldLabel);

            if (data.site != null)
            {
                InterlockedSections();
            }

            EditorGUILayout.Space(25);

            if (EditorGUI.EndChangeCheck())
            {
                bodyGrid = ConvertTilemapToBoolArray(data.tileMap);
                EditorUtility.SetDirty(target);
            }
            
            DrawAllGridLayout();
        }

        private void DrawAllGridLayout()
        {
            if (data.upsideData != null)
            {
                var tiles = ConvertTilemapToBoolArray(data.upsideData.tileMap);
                DrawTilemap(tiles, interlockColor);
            }

            EditorGUILayout.Space(10);

            DrawTilemap(bodyGrid, fillColor);

            EditorGUILayout.Space(10);

            if (data.downsideData != null)
            {
                var tiles = ConvertTilemapToBoolArray(data.downsideData.tileMap);
                DrawTilemap(tiles, interlockColor);
            }
        }

        private void InterlockedSections()
        {
            switch ((data.site.upside, data.site.downside))
            {
                case (not null, null):
                    data.upsideData = EditorGUILayout.ObjectField("Upside Data", data.upsideData, typeof(SectionDataSO), false) as SectionDataSO;
                    data.downsideData = null;
                    break;

                case (not null, not null):
                    data.upsideData = EditorGUILayout.ObjectField("Upside Data", data.upsideData, typeof(SectionDataSO), false) as SectionDataSO;
                    data.downsideData = EditorGUILayout.ObjectField("Downside Data", data.downsideData, typeof(SectionDataSO), false) as SectionDataSO;
                    break;

                case (null, not null):
                    data.upsideData = null;
                    data.downsideData = EditorGUILayout.ObjectField("Upside Data", data.downsideData, typeof(SectionDataSO), false) as SectionDataSO;
                    break;
            }
        }

        private void DrawTilemap(bool[,] grid, Color cellColor)
        {
            if (data.tileMap == null)
                return;

            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            float availableWidth = EditorGUIUtility.currentViewWidth - PADDING;
            float cellSize = Mathf.Clamp(availableWidth / cols, MIN_CELL_SIZE, MAX_CELL_SIZE);

            float totalWidth = cols * cellSize;
            float totalHeight = rows * cellSize;

            Rect gridRect = GUILayoutUtility.GetRect(totalWidth, totalHeight);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Rect cellRect = new Rect(
                                    gridRect.x + j * cellSize,
                                    gridRect.y + (rows - 1 - i) * cellSize,
                                    cellSize,
                                    cellSize);

                    EditorGUI.DrawRect(cellRect, grid[i, j] ? cellColor : emptyColor);

                    Handles.color = Color.gray;
                    Handles.DrawLine(new Vector3(cellRect.x, cellRect.y), new Vector3(cellRect.x + cellSize, cellRect.y));
                    Handles.DrawLine(new Vector3(cellRect.x, cellRect.y), new Vector3(cellRect.x, cellRect.y + cellSize));
                }
            }

            for (int i = 0; i < rows; i++)
            {
                Rect lastCell = new Rect(gridRect.x + (cols - 1) * cellSize, gridRect.y + (rows - 1 - i) * cellSize, cellSize, cellSize);
                Handles.DrawLine(new Vector3(lastCell.x + cellSize, lastCell.y), new Vector3(lastCell.x + cellSize, lastCell.y + cellSize));
            }
            for (int j = 0; j < cols; j++)
            {
                Rect lastCell = new Rect(gridRect.x + j * cellSize, gridRect.y + (rows - 1) * cellSize, cellSize, cellSize);
                Handles.DrawLine(new Vector3(lastCell.x, lastCell.y + cellSize), new Vector3(lastCell.x + cellSize, lastCell.y + cellSize));
            }
        }

        private bool[,] ConvertTilemapToBoolArray(Tilemap tilemap)
        {
            if (tilemap == null)
                return new bool[0, 0];

            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;

            int width = bounds.size.x;
            int height = bounds.size.y;
            bool[,] grid = new bool[height, width];

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    int arrayX = x - bounds.xMin;
                    int arrayY = y - bounds.yMin;

                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    grid[arrayY, arrayX] = tilemap.HasTile(tilePosition);
                }
            }
            return grid;
        }
    }
}