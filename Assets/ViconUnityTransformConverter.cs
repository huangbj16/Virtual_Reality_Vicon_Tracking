using UnityEngine;
using System.Diagnostics;
using UnityVicon;
public class ViconUnityTransformConverter : MonoBehaviour
{

  public GameObject objectCollection;
  public Matrix4x4 coordinatesRotationMatrix;

  public Vector3 translateVector;

  public Transform leftController;

  public bool converterIsOn = false;

  public Stopwatch st = new Stopwatch();


  void ApplyRotationToChild(Transform objectTransform, Transform unityTransform, Matrix4x4 matrix )
    {
        if (objectTransform.childCount > 0)
        {
            foreach(Transform child in objectTransform)
            {
                ApplyRotationToChild(child, unityTransform.FindChildRecursive(child.name), matrix);
            }
        }
        // unityTransform.position = coordinatesRotationMatrix.MultiplyPoint3x4(objectTransform.position) + translateVector;
        unityTransform.localPosition = objectTransform.localPosition;
        unityTransform.localRotation = objectTransform.localRotation;

    }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (!converterIsOn) return;

    foreach (Transform viconTransform in objectCollection.transform)
    {
        Transform unityTransform;
        if (viconTransform.GetComponent<SubjectScript>())
        {
            Transform hips = viconTransform.FindChildRecursive("Hips");
      
            unityTransform = hips.GetComponent<GetUnityTransform>().unityTransform;
            
            ApplyRotationToChild(hips, unityTransform, coordinatesRotationMatrix);

            Matrix4x4 oldRotation = Matrix4x4.TRS(Vector3.zero, viconTransform.rotation, Vector3.one);
            Matrix4x4 combinedMatrix = coordinatesRotationMatrix * oldRotation;
            unityTransform.rotation = Quaternion.LookRotation(combinedMatrix.GetColumn(2), combinedMatrix.GetColumn(1));
            unityTransform.position = coordinatesRotationMatrix.MultiplyPoint3x4(hips.position) + translateVector;

        }
        else
        {
                //Whatever needs timing here
                unityTransform = viconTransform.GetComponent<GetUnityTransform>().unityTransform;
                // calculate new transform with vicon transform and calibration matr(ix.
                Matrix4x4 oldRotation = Matrix4x4.TRS(Vector3.zero, viconTransform.rotation, Vector3.one);
                Matrix4x4 combinedMatrix = coordinatesRotationMatrix * oldRotation;
                unityTransform.rotation = Quaternion.LookRotation(combinedMatrix.GetColumn(2), combinedMatrix.GetColumn(1));
            
            //unityTransform.position = Matrix4x4.identity.MultiplyPoint3x4(leftController.position) + new Vector3(0, 0, 0);

            unityTransform.position = coordinatesRotationMatrix.MultiplyPoint3x4(viconTransform.position) + translateVector;
        }
     
    }
        
  }

}
