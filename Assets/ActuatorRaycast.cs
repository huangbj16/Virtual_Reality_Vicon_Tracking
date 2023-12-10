using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActuatorRaycast : MonoBehaviour
{
    public Transform anchorPoint; // the anchor point which defines the direction
    public Transform point; // Your point with position and rotation
    private int motor_id = -1;
    private Material highlightMaterial;
    private Material defaultMaterial;
    private float maxDistance = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        point = this.transform;
        motor_id = this.GetComponent<HapticMotor>().motor_id;
        highlightMaterial = Resources.Load<Material>("Materials/Material_Red");
        defaultMaterial = this.GetComponent<Renderer>().material;
    }

    void Update()
    {
        RaycastHit hit;
        // Ray direction is the forward direction of the point
        Vector3 direction = Vector3.Normalize(point.position - anchorPoint.position);
        // Debug.Log(point.position + " " + direction);

        // Perform the raycast
        if (Physics.Raycast(point.position, direction, out hit, maxDistance))
        {
            if (hit.collider.gameObject.name != "Plane")
            {
                Debug.Log(this.name + motor_id + " Hit " + hit.collider.gameObject.name + ", Distance = " + hit.distance);
                Debug.DrawRay(point.position, direction * hit.distance, Color.red);
                this.GetComponent<Renderer>().material = highlightMaterial;
            }
            else
            {
                Debug.DrawRay(point.position, direction * hit.distance, Color.white);
                this.GetComponent<Renderer>().material = defaultMaterial;
            }
        }
        else
        {
            Debug.DrawRay(point.position, direction * maxDistance, Color.white);
            this.GetComponent<Renderer>().material = defaultMaterial;
        }
    }
}
