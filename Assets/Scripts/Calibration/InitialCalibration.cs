using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCalibration : MonoBehaviour
{
  // Start is called before the first frame update
  public Transform unityTransform;

  public Transform viconTransform;

  private List<Vector3[]> dataCollection = new List<Vector3[]>();

  private Vector3 previousViconPosition = Vector3.zero;
  private Vector3 previousUnityPosition = Vector3.zero;

  private Matrix4x4 viconPointsMatrix = Matrix4x4.identity;
  private Matrix4x4 unityPointsMatrix = Matrix4x4.identity;

  private Matrix4x4 coordinatesRotationMatrix = Matrix4x4.identity;


  public void CaptureData()
  {
    Vector3 unityPositionDelta = unityTransform.position - previousUnityPosition;
    Vector3 viconPositionDelta = viconTransform.position - previousViconPosition;
    if (unityPositionDelta.magnitude > 0.05 && viconPositionDelta.magnitude > 0.05)
    {

      dataCollection.Add([unityPositionDelta.normalized, viconPositionDelta.normalized]);
      Debug.Log(unityPositionDelta.magnitude + ", " + viconPositionDelta.magnitude + ", " + (unityPositionDelta.magnitude - viconPositionDelta.magnitude));

      Debug.Log("unity pos change norm:" + Vector3.Normalize(unityPositionDelta).ToString("F4") + "vicon pos change norm: " + Vector3.Normalize(viconPositionDelta).ToString("F4"));
    }
    previousUnityPosition = unityTransform.position;
    previousViconPosition = viconTransform.position;
  }

  public void JustDoIt()
  {
    Debug.Log("just do it ！");
  }
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  // getter and setter
  public Matrix4x4 getCoordinatesRotationMatrix()
  {
    return coordinatesRotationMatrix;
  }
}
