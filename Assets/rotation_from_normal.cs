using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation_from_normal : MonoBehaviour
{
    public Vector3 normal_vector;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localRotation = Quaternion.FromToRotation(transform.up, normal_vector) * this.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
