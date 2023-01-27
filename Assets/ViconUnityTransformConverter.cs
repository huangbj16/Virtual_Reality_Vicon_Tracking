using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class ViconUnityTransformConverter : MonoBehaviour
{

  public GameObject objectCollection;
  public Matrix4x4 coordinatesRotationMatrix;

  public Vector3 translateVector;

  public bool converterIsOn = false;

  // // for testing
  // // test set 1
  // public Vector3 u1 = new Vector3(-0.8281f, -0.3756f, -0.4161f);
  // public Vector3 u2 = new Vector3(-0.4542f, -0.4135f, 0.7891f);
  // public Vector3 u3 = new Vector3(0.8005f, 0.0932f, -0.5920f);

  // public Vector3 v1 = new Vector3(0.9072f, -0.1099f, -0.4061f);
  // public Vector3 v2 = new Vector3(0.1254f, 0.9208f, -0.3694f);
  // public Vector3 v3 = new Vector3(-0.5670f, -0.8179f, 0.0980f);

public Stopwatch st = new Stopwatch();

    // Start is called before the first frame update
    void Start()
  {

  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (!converterIsOn) return;

    foreach (Transform viconTransform in objectCollection.transform)
    {
        
        //Whatever needs timing here
        Transform unityTransform = viconTransform.GetComponent<GetUnityTransform>().unityTransform;
            /**
        // calculate new transform with vicon transform and calibration matrix.
        Vector3 forward = viconTransform.transform.forward;
        Vector3 up = unityTransform.up;
        // need to check if here is correct, since MultiplePoint3x4 is newly introduced.
        Vector3 forward_conv = coordinatesRotationMatrix.MultiplyPoint3x4(forward);
        Vector3 up_conv = coordinatesRotationMatrix.MultiplyPoint3x4(up);
        unityTransform.rotation = Quaternion.LookRotation(forward_conv, up_conv);
        **/
            unityTransform.position = coordinatesRotationMatrix.MultiplyPoint3x4(viconTransform.position) + translateVector;
        }
        
    }

  // void test matrix rotation function

  // void Test()
  // {
  //   Matrix4x4 unityMatrix = Matrix4x4.identity;
  //   unityMatrix.SetRow(0, new Vector4(u1.x, u1.y, u1.z, 0));
  //   unityMatrix.SetRow(1, new Vector4(u2.x, u2.y, u2.z, 0));
  //   unityMatrix.SetRow(2, new Vector4(u3.x, u3.y, u3.z, 0));
  //   unityMatrix = unityMatrix.transpose;

  //   Matrix4x4 viconMatrix = Matrix4x4.identity;
  //   viconMatrix.SetRow(0, new Vector4(v1.x, v1.y, v1.z, 0));
  //   viconMatrix.SetRow(1, new Vector4(v2.x, v2.y, v2.z, 0));
  //   viconMatrix.SetRow(2, new Vector4(v3.x, v3.y, v3.z, 0));

  //   viconMatrix = viconMatrix.transpose;

  //   Matrix4x4 viconMatrixInverse = viconMatrix.inverse;
  //   // A = X' * X^-1
  //   Matrix4x4 RotationMatrix = unityMatrix * viconMatrixInverse;
  // }

}
