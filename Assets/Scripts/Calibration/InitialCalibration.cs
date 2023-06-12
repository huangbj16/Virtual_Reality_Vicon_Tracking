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
    private CalibrationData[] topThreeLeastDifference = new CalibrationData[3];

    private Vector3 anchorUnityPosition = Vector3.zero;
    private Vector3 anchorViconPosition = Vector3.zero;
    private float differenceThreshold = 0.01f; // change if need
    private float magnitureThreshold = 0.05f; // change if need


    private Matrix4x4 coordinatesRotationMatrix = Matrix4x4.identity;

    private Vector3 translationVector3 = Vector3.zero;

    public ViconUnityTransformConverter dataConverter;

    // setup the anchor point for both coordinates, used for following comparsions.
    public void CaptureAnchor()
    {
        anchorUnityPosition = unityTransform.position;
        anchorViconPosition = viconTransform.position;
    }

    // capture follow-up data, calculate relative difference regarding to anchor points, save to input matrices if under threshold
    public void CaptureData()
    {
        Vector3 unityPositionDelta = unityTransform.position - anchorUnityPosition;
        Vector3 viconPositionDelta = viconTransform.position - anchorViconPosition;
        float difference = Mathf.Abs(unityPositionDelta.magnitude - viconPositionDelta.magnitude);
        Debug.Log("capture data diff = "+difference);
        if (unityPositionDelta.magnitude > magnitureThreshold && viconPositionDelta.magnitude > magnitureThreshold && difference < differenceThreshold)
        {
            Debug.Log("add new calibration data");
            CalibrationData data = new CalibrationData(viconPositionDelta.normalized, unityPositionDelta.normalized, difference);
            dataCollection.Add(data);
            UpdateInputMatrices(data);
        }
    }

    // compare the new data with existing data, only keep the one with smaller differnce.
    private void UpdateInputMatrices(CalibrationData newData)
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

    public Matrix4x4 CalculateRotationMatrix()
    {
        Matrix4x4 viconMatrix = Matrix4x4.identity;
        Matrix4x4 unityMatrix = Matrix4x4.identity;

        viconMatrix.SetRow(0, new Vector4(topThreeLeastDifference[0].viconDelta.x, topThreeLeastDifference[0].viconDelta.y, topThreeLeastDifference[0].viconDelta.z, 0));
        viconMatrix.SetRow(1, new Vector4(topThreeLeastDifference[1].viconDelta.x, topThreeLeastDifference[1].viconDelta.y, topThreeLeastDifference[1].viconDelta.z, 0));
        viconMatrix.SetRow(2, new Vector4(topThreeLeastDifference[2].viconDelta.x, topThreeLeastDifference[2].viconDelta.y, topThreeLeastDifference[2].viconDelta.z, 0));
        viconMatrix = viconMatrix.transpose;

        unityMatrix.SetRow(0, new Vector4(topThreeLeastDifference[0].unityDelta.x, topThreeLeastDifference[0].unityDelta.y, topThreeLeastDifference[0].unityDelta.z, 0));
        unityMatrix.SetRow(1, new Vector4(topThreeLeastDifference[1].unityDelta.x, topThreeLeastDifference[1].unityDelta.y, topThreeLeastDifference[1].unityDelta.z, 0));
        unityMatrix.SetRow(2, new Vector4(topThreeLeastDifference[2].unityDelta.x, topThreeLeastDifference[2].unityDelta.y, topThreeLeastDifference[2].unityDelta.z, 0));
        unityMatrix = unityMatrix.transpose;

        Matrix4x4 viconMatrixInverse = viconMatrix.inverse;
        // A = X' * X^-1
        coordinatesRotationMatrix = unityMatrix * viconMatrixInverse;
        CalculateTranslationVector();

        dataConverter.coordinatesRotationMatrix = coordinatesRotationMatrix;
        dataConverter.translateVector = translationVector3;
        return getCoordinatesRotationMatrix();
    }

    public Vector3 CalculateTranslationVector()
    {
        // can be proved by use topLeastDifferenceData to reduce bias if necessary
        Vector3 positionWithCoordinatesRotation = coordinatesRotationMatrix.MultiplyPoint3x4(viconTransform.position);
        translationVector3 = unityTransform.position - positionWithCoordinatesRotation;
        return getTranslationVector();
    }

    public Vector3 getTranslationVector()
    {
        Debug.Log("---- coordinate translate vector ----");
        Debug.Log(translationVector3.ToString());
        return translationVector3;
    }
    // getter and setter
    public Matrix4x4 getCoordinatesRotationMatrix()
    {
        Debug.Log("---- coordinate rotation matrix ----");
        Debug.Log(coordinatesRotationMatrix.ToString());
        return coordinatesRotationMatrix;
    }

    void Start()
    {
        // initialize the data array
        topThreeLeastDifference[0] = new CalibrationData(Vector3.zero, Vector3.zero, float.MaxValue - 2);
        topThreeLeastDifference[1] = new CalibrationData(Vector3.zero, Vector3.zero, float.MaxValue - 1);
        topThreeLeastDifference[2] = new CalibrationData(Vector3.zero, Vector3.zero, float.MaxValue);
    }
}
