using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InitialCalibration))]
[CanEditMultipleObjects]
public class CustomInspector : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
    InitialCalibration calibration = (InitialCalibration)target;
    if (GUILayout.Button("Capture Motion"))
    {
      calibration.JustDoIt();
    }
  }
}
