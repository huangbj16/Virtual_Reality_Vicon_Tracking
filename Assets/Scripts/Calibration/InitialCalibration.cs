using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCalibration : MonoBehaviour
{
  // Start is called before the first frame update

  public class CalibrationData
  {
    public Vector3 viconDelta;
    public Vector3 unityDelta;
    public float difference;
    public CalibrationData(Vector3 vicon, Vector3 unity, float diff)
    {
      this.viconDelta = vicon;
      this.unityDelta = unity;
      this.difference = diff;
    }
  }
  public Transform unityTransform;

  public Transform viconTransform;

  private List<CalibrationData> dataCollection = new List<CalibrationData>();

  public CalibrationData[] topThreeLeastDifference = new CalibrationData[3];

  private Vector3 previousViconPosition = Vector3.zero;
  private Vector3 previousUnityPosition = Vector3.zero;


  public Matrix4x4 coordinatesRotationMatrix = Matrix4x4.identity;


  public void CaptureData()
  {
    Vector3 unityPositionDelta = unityTransform.position - previousUnityPosition;
    Vector3 viconPositionDelta = viconTransform.position - previousViconPosition;
    if (unityPositionDelta.magnitude > 0.05 && viconPositionDelta.magnitude > 0.05)
    {

      float difference = Mathf.Abs(unityPositionDelta.magnitude - viconPositionDelta.magnitude);
      CalibrationData data = new CalibrationData(viconPositionDelta.normalized, unityPositionDelta.normalized, difference);
      dataCollection.Add(data);

    }
    previousUnityPosition = unityTransform.position;
    previousViconPosition = viconTransform.position;
  }

  private void UpdateInputMatrices(Matrix4x4 viconMatrix, Matrix4x4 unityMatrix, CalibrationData newData)
  {
    if (newData.difference < topThreeLeastDifference[0].difference)
    {
      topThreeLeastDifference[2] = topThreeLeastDifference[1];
      topThreeLeastDifference[1] = topThreeLeastDifference[0];
      topThreeLeastDifference[0] = newData;
    }
    else if (newData.difference < topThreeLeastDifference[1].difference)
    {
      topThreeLeastDifference[2] = topThreeLeastDifference[1];
      topThreeLeastDifference[1] = newData;
    }
    else if (newData.difference < topThreeLeastDifference[2].difference)
    {
      topThreeLeastDifference[2] = newData;
    }
  }

  public void CalculateRotationMatrix()
  {
    Matrix4x4 viconMatrix = Matrix4x4.zero;
    viconMatrix.SetRow(0, new Vector4(topThreeLeastDifference[0].viconDelta.x, topThreeLeastDifference[0].viconDelta.y, topThreeLeastDifference[0].viconDelta.z, 0));
    viconMatrix.SetRow(1, new Vector4(topThreeLeastDifference[1].viconDelta.x, topThreeLeastDifference[1].viconDelta.y, topThreeLeastDifference[1].viconDelta.z, 0));
    viconMatrix.SetRow(2, new Vector4(topThreeLeastDifference[2].viconDelta.x, topThreeLeastDifference[2].viconDelta.y, topThreeLeastDifference[2].viconDelta.z, 0));

    Matrix4x4 unityMatrix = Matrix4x4.zero;
    unityMatrix.SetRow(0, new Vector4(topThreeLeastDifference[0].unityDelta.x, topThreeLeastDifference[0].unityDelta.y, topThreeLeastDifference[0].unityDelta.z, 0));
    unityMatrix.SetRow(1, new Vector4(topThreeLeastDifference[1].unityDelta.x, topThreeLeastDifference[1].unityDelta.y, topThreeLeastDifference[1].unityDelta.z, 0));
    unityMatrix.SetRow(2, new Vector4(topThreeLeastDifference[2].unityDelta.x, topThreeLeastDifference[2].unityDelta.y, topThreeLeastDifference[2].unityDelta.z, 0));

    Matrix4x4 inverseViconMatrix = viconMatrix.inverse;
    coordinatesRotationMatrix = unityMatrix * inverseViconMatrix.transpose;

  }
  void Start()
  {
    topThreeLeastDifference[0] = new CalibrationData(Vector3.zero, Vector3.zero, float.MaxValue - 2);
    topThreeLeastDifference[1] = new CalibrationData(Vector3.zero, Vector3.zero, float.MaxValue - 1);
    topThreeLeastDifference[2] = new CalibrationData(Vector3.zero, Vector3.zero, float.MaxValue);

  }

  // getter and setter
  public Matrix4x4 getCoordinatesRotationMatrix()
  {
    return coordinatesRotationMatrix;
  }
}
