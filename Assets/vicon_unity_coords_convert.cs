using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vicon_unity_coords_convert : MonoBehaviour
{
    public GameObject sphere;
    public GameObject leftController;
    private Vector3 translationVector;
    private Vector3 A1 = new Vector3(-0.9148538f, -0.33422382f, 0.08587991f);
    private Vector3 A2 = new Vector3(0.06135099f, -0.02832116f, 1.06961368f);
    private Vector3 A3 = new Vector3(-0.29500871f, 0.94158979f, 0.11077906f);
    

    public Vector3 previousViconPosition = Vector3.zero;
    public Vector3 previousUnityPosition = Vector3.zero;

    public float timegap;
    public float timeRemaining;
 
   
    // Start is called before the first frame update

    void Start()
    {

        StartCoroutine(CalculateTranform());
        timeRemaining = timegap;
    }

    // Update is called once per frame
    void Update()
    {
        //sphere.transform.rotation = this.transform.rotation * rotationQuaternion;
        //sphere.transform.position = this.transform.position + translationVector;
        Vector3 forward = this.transform.forward;
        Vector3 up = this.transform.up;
        Vector3 forward_conv = new Vector3(Vector3.Dot(A1, forward), Vector3.Dot(A2, forward), Vector3.Dot(A3, forward));
        Vector3 up_conv = new Vector3(Vector3.Dot(A1, up), Vector3.Dot(A2, up), Vector3.Dot(A3, up));
        sphere.transform.rotation = Quaternion.LookRotation(forward_conv, up_conv);
        sphere.transform.position = new Vector3(Vector3.Dot(A1, this.transform.position), Vector3.Dot(A2, this.transform.position), Vector3.Dot(A3, this.transform.position)) + translationVector;


        /*
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            return;
        }

        timeRemaining = timegap;
        

        Vector3 unityPositionDelta = leftController.transform.position - previousUnityPosition;
        Vector3 viconPositionDelta = this.transform.position - previousViconPosition;
        if (unityPositionDelta.magnitude > 0.05  && viconPositionDelta.magnitude > 0.05)
        {
            Debug.Log(unityPositionDelta.magnitude + ", " + viconPositionDelta.magnitude + ", " + (unityPositionDelta.magnitude - viconPositionDelta.magnitude));

            Debug.Log("unity pos change norm:" + Vector3.Normalize(unityPositionDelta).ToString("F4") + "vicon pos change norm: " + Vector3.Normalize(viconPositionDelta).ToString("F4"));
        }

      


        previousUnityPosition = leftController.transform.position;
        previousViconPosition = this.transform.position;

        timeRemaining -= Time.deltaTime;
        */

    }


    IEnumerator CalculateTranform()
    {
        yield return new WaitForSeconds(3);
//        Debug.Log(this.transform.forward.ToString("F4") + ", " + this.transform.up.ToString("F4"));
        Vector3 temp = new Vector3(Vector3.Dot(A1, this.transform.position), Vector3.Dot(A2, this.transform.position), Vector3.Dot(A3, this.transform.position));
        translationVector = leftController.transform.position - temp;

    }
}
