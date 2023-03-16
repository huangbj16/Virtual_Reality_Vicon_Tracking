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
    ShapeGenerator ohShapeGenerator = (ShapeGenerator)target;
    GUILayout.Label("Enter a new direction and click 'Reset Direction'");
    newDirection = EditorGUILayout.Vector3Field("New Direction", newDirection);

    if (GUILayout.Button("Reset Direction"))
    {
      ohShapeGenerator.ResetDirection(newDirection);
    }
  }
}
