using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    [CustomEditor(typeof(SectionDataSO))]
    public class SectionDataSOEditor : Editor
    {
        private SectionDataSO block;
        private bool[,] grid;
        private Color trueColor = Color.black;
        private Color falseColor = Color.white;

        private const float MIN_CELL_SIZE = 4;
        private const float MAX_CELL_SIZE = 50f;
        private const float PADDING = 40f;

        private void OnEnable()
        {
            block = (SectionDataSO)target;
            grid = ConvertTilemapToBoolArray(block.tileMap);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Block Settings", EditorStyles.boldLabel);

            block.location = (SectionLocation)EditorGUILayout.EnumPopup("Location", block.location);

            switch (block.location)
            {
                case SectionLocation.Bottom:
                    block.upside = EditorGUILayout.ObjectField("Upside Type", block.upside, typeof(SectionInterlock), false) as SectionInterlock;
                    block.downside = null;
                    break;

                case SectionLocation.Middle:
                    block.upside = EditorGUILayout.ObjectField("Upside Type", block.upside, typeof(SectionInterlock), false) as SectionInterlock;
                    block.downside = EditorGUILayout.ObjectField("Downside Type", block.downside, typeof(SectionInterlock), false) as SectionInterlock;
                    break;

                case SectionLocation.Top:
                    block.upside = null;
                    block.downside = EditorGUILayout.ObjectField("Upside Type", block.downside, typeof(SectionInterlock), false) as SectionInterlock;
                    break;
            }

            EditorGUILayout.Space(25);
            DrawTilemap();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawTilemap()
        {
            if (block.tileMap == null)
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

                    EditorGUI.DrawRect(cellRect, grid[i, j] ? trueColor : falseColor);

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