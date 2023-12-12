using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinePositionMatching : MonoBehaviour
{
    public GameObject referencePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (referencePoint != null)
        {
            this.transform.position = referencePoint.transform.position;
            this.transform.rotation = referencePoint.transform.rotation;
        }
        else
        {
            referencePoint = GameObject.Find("Joint Chest");
        }
    }
}
