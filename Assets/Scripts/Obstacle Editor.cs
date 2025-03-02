using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleEditor : EditorWindow
{
    private ObstacleData _obstacleData;
    private List<bool> _editorGrid = new(new bool[100]);
    
    [MenuItem("Window/Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditor>("Obstacle Editor");
    }

    private void OnEnable()
    {
        LoadObstacleData();
    }

    private void OnGUI()
    {
        if (EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox("Editing is disabled during runtime.", MessageType.Warning);
            return;
        }
        
        EditorGUILayout.BeginHorizontal();
        _obstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", _obstacleData, typeof(ObstacleData), false);
        if (GUILayout.Button("New Obstacle Data"))
        {
            CreateNewObstacleData();
        }
        EditorGUILayout.EndHorizontal();

        if (!_obstacleData)
        {
            EditorGUILayout.HelpBox("No ObstacleData selected. Create or load one to proceed.", MessageType.Warning);
            return;
        }

        _editorGrid = _obstacleData.GetObstacleGrid();
        EditorGUILayout.LabelField("Obstacle Grid", EditorStyles.boldLabel);
        for (var x = 0; x < 10; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (var y = 0; y < 10; y++)
            {
                _editorGrid[x * 10 + y] = EditorGUILayout.Toggle(_editorGrid[x * 10 + y]);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Obstacles"))
        {
            SaveObstacleData();
        }
    }
    
    private void LoadObstacleData()
    {
        var guids = AssetDatabase.FindAssets("t:ObstacleData");
        if (guids.Length <= 0) return;
        
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        _obstacleData = AssetDatabase.LoadAssetAtPath<ObstacleData>(path);
    }

    private void CreateNewObstacleData()
    {
        _obstacleData = CreateInstance<ObstacleData>();

        var path = EditorUtility.SaveFilePanelInProject("Save Obstacle Data", "NewObstacleData", "asset", "Save Obstacle Data");
        if (string.IsNullOrEmpty(path)) return;
        
        AssetDatabase.CreateAsset(_obstacleData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void SaveObstacleData()
    {
        if (!_obstacleData) return;
        
        for (var x = 0; x < 10; x++)
        {
            for (var y = 0; y < 10; y++)
            {
                _obstacleData.SetTile(x, y, _editorGrid[x * 10 + y]);
            }
        }
        EditorUtility.SetDirty(_obstacleData);
        AssetDatabase.SaveAssets();
    }
}