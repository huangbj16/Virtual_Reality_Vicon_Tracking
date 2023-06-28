using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachHapticMotors : MonoBehaviour
{
  public Transform start;
  public Transform end;

  public int numberOfMotors;

  public List<Transform> hapticMotors = new List<Transform>();

  public Vector3 offset;
  // Start is called before the first frame update


  public static GameObject hapticMotorPrefab;
  void Start()
  {
    // load prefabs;
    hapticMotorPrefab = Resources.Load<GameObject>("Prefabs/HapticMotor");

    for (int i = 0; i < numberOfMotors; i++)
    {
      GameObject newMotor = Instantiate(hapticMotorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
      hapticMotors.Add(newMotor.transform);
    }
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 step = (end.position - start.position) / (numberOfMotors + 1);
    for (int i = 0; i < numberOfMotors; i++)
    {
      Transform motor = hapticMotors[i];
      motor.position = start.position + (i + 1) * step;
    }
  }
}
