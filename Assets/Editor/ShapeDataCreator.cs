using UnityEngine;
using UnityEditor;

public class ShapeDataCreator : EditorWindow
{
    private int gridSize = 5;
    private bool[,] grid;
    private string shapeName = "NewShape";
    private ColorPallete colorPallete;

    [MenuItem("Tools/Create Shape Data")]
    public static void ShowWindow()
    {
        GetWindow<ShapeDataCreator>("Shape Data Creator");
    }

    private void OnEnable()
    {
        grid = new bool[gridSize, gridSize];
    }

    private void OnGUI()
    {
        GUILayout.Label("Shape Data Creator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Tên shape
        shapeName = EditorGUILayout.TextField("Shape Name", shapeName);

        // Chọn ColorPallete
        colorPallete = (ColorPallete)EditorGUILayout.ObjectField(
            "Color Pallete", colorPallete, typeof(ColorPallete), false);

        EditorGUILayout.Space();
        GUILayout.Label($"Grid ({gridSize}×{gridSize})", EditorStyles.boldLabel);

        // Vẽ bảng lưới
        for (int row = 0; row < gridSize; row++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int col = 0; col < gridSize; col++)
            {
                bool current = grid[row, col];
                Color originalColor = GUI.backgroundColor;
                GUI.backgroundColor = current ? Color.green : Color.gray;

                if (GUILayout.Button("", GUILayout.Width(40), GUILayout.Height(40)))
                {
                    grid[row, col] = !grid[row, col];
                }
                GUI.backgroundColor = originalColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }
        if (GUILayout.Button("Export Shape Data"))
        {
            ExportShapeData();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ClearGrid()
    {
        for (int r = 0; r < gridSize; r++)
            for (int c = 0; c < gridSize; c++)
                grid[r, c] = false;
    }

    private void ExportShapeData()
    {
        if (string.IsNullOrEmpty(shapeName))
        {
            EditorUtility.DisplayDialog("Error", "Please enter a shape name.", "OK");
            return;
        }

        // Tạo ShapeData
        ShapeData shapeData = ScriptableObject.CreateInstance<ShapeData>();
        shapeData.rows = gridSize;
        shapeData.columns = gridSize;
        shapeData.shapeBoard = new ShapeData.Row[gridSize];

        for (int r = 0; r < gridSize; r++)
        {
            shapeData.shapeBoard[r] = new ShapeData.Row(gridSize);
            for (int c = 0; c < gridSize; c++)
            {
                shapeData.shapeBoard[r].colums[c] = grid[r, c];
            }
        }
        shapeData.colorPallete = colorPallete;

        // Chọn nơi lưu
        string path = EditorUtility.SaveFilePanel("Save Shape Data", "Assets", shapeName, "asset");
        if (string.IsNullOrEmpty(path))
            return;

        // Chuyển sang đường dẫn tương đối với Assets
        if (path.StartsWith(Application.dataPath))
        {
            path = "Assets" + path.Substring(Application.dataPath.Length);
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Path must be inside the Assets folder.", "OK");
            return;
        }

        AssetDatabase.CreateAsset(shapeData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"Shape Data created at: {path}", "OK");
        Selection.activeObject = shapeData;
    }
}