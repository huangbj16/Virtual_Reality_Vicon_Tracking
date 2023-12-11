using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActuatorRaycast : MonoBehaviour
{
    public bool isTriggered = false;
    public Transform anchorPoint; // the anchor point which defines the direction
    public Transform point; // Your point with position and rotation
    private int motor_id = -1;
    private Material highlightMaterial;
    private Material defaultMaterial;
    private float maxDistance = 2.0f;

    private GameObject senderObject;
    private TcpSender sender;
    public Dictionary<string, int> command;

    // Start is called before the first frame update
    void Start()
    {
        point = this.transform;
        motor_id = this.GetComponent<HapticMotor>().motor_id;
        highlightMaterial = Resources.Load<Material>("Materials/Material_Red");
        defaultMaterial = this.GetComponent<Renderer>().material;

        senderObject = GameObject.Find("TCPSenderObject");
        sender = senderObject.GetComponent<TcpSender>();
        command = new Dictionary<string, int>()
        {
            { "addr", motor_id },
            { "mode", 0 },
            { "duty", 15 },
            { "freq", 2 },
            { "wave", 0 }
        };
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
                StartVibrate();
            }
            else
            {
                Debug.DrawRay(point.position, direction * hit.distance, Color.white);
                this.GetComponent<Renderer>().material = defaultMaterial;
                StopVibrate();
            }
        }
        else
        {
            Debug.DrawRay(point.position, direction * maxDistance, Color.white);
            this.GetComponent<Renderer>().material = defaultMaterial;
            StopVibrate();
        }
    }


    public string DictionaryToString(Dictionary<string, int> dictionary)
    {
        string dictionaryString = "{";
        foreach (KeyValuePair<string, int> keyValues in dictionary)
        {
            dictionaryString += "\"" + keyValues.Key + "\": " + keyValues.Value + ", ";
        }
        return dictionaryString.TrimEnd(',', ' ') + "}";
    }

    public void StartVibrate()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            // send TCP command
            Debug.Log("Send start command to PORT:9051");
            command["mode"] = 1;
            string commandString = DictionaryToString(command) + "\n";
            Debug.Log(commandString);
            sender.SendData(commandString);
            // collusionData = new CollusionData(motor_id, other);
        }
    }

    public void Vibrate()
    {
        // trigger some animation.

    }

    public void StopVibrate()
    {
        if (isTriggered)
        {
            isTriggered = false;
            //send TCP command
            Debug.Log("Send stop command to PORT:9051");
            command["mode"] = 0;
            string commandString = DictionaryToString(command) + "\n";
            Debug.Log(commandString);
            sender.SendData(commandString);
            sender.SendData(commandString);
            // collusionData.detail = detail;
            // collusionData.CalculateCollusionDuration();
            // Debug.Log(collusionData.actutorId + ": " + other);
            // shapeGenerator.currentUserData.TriggerAlert(collusionData);
            // add event handler to trigger ohshape generator
        }
    }
}
