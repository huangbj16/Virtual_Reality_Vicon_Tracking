using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeGenerator))]
[CanEditMultipleObjects]
public class OhShapeGeneratorInspector : Editor
{

  public Vector3 newDirection;

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    GUIStyle labelLayout = new GUIStyle(GUI.skin.label)
    {
      fontSize = 12,
      fontStyle = FontStyle.Bold
    };

    ShapeGenerator ohShapeGenerator = (ShapeGenerator)target;
    GUILayout.Label("Enter a new direction and click 'Reset Direction'", labelLayout);
    newDirection = EditorGUILayout.Vector3Field("New Direction", newDirection);

    if (GUILayout.Button("Reset Direction"))
    {
      ohShapeGenerator.ResetDirection(newDirection);
    }

    GUILayout.Label("Current Mode: " + ohShapeGenerator.currentConfig.mode, labelLayout);


    GUILayoutOption[] modeButtonLayout = new GUILayoutOption[] { GUILayout.Height(80) };


    GUILayout.BeginHorizontal();
    if (GUILayout.Button("Practice Mode", modeButtonLayout))
    {
      ohShapeGenerator.SetMode(ShapeGeneratorConstants.PRACTICE_MODE);
    }

    if (GUILayout.Button("Clean Mode", modeButtonLayout))
    {
      ohShapeGenerator.SetMode(ShapeGeneratorConstants.CLEAN_MODE);
    }

    if (GUILayout.Button("Visual Mode", modeButtonLayout))
    {
      ohShapeGenerator.SetMode(ShapeGeneratorConstants.VISUAL_MODE);
    }

    if (GUILayout.Button("Haptic Mode", modeButtonLayout))
    {
      ohShapeGenerator.SetMode(ShapeGeneratorConstants.HAPTIC_MODE);
    }

    GUILayout.EndHorizontal();


    GUILayout.BeginHorizontal();
    if (GUILayout.Button("Start", modeButtonLayout))
    {
      ohShapeGenerator.StartGame();
    }

    // if (GUILayout.Button("Pause", modeButtonLayout))
    // {
    //   ohShapeGenerator.PauseGame();
    // }
    GUILayout.EndHorizontal();
  }
}
