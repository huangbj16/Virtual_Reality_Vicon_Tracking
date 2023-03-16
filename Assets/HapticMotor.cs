using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticMotor : MonoBehaviour
{

  public bool isTriggered = false;

  private float endTime;

  public float vibrationTime = 1.0f;

  public int intensity = 1;


  public void StartVibrate()
  {
    endTime = Time.time + vibrationTime;
    isTriggered = true;
    // send data to certain port....
    Debug.Log("Send start command to PORT:1000");
  }

  public void Vibrate()
  {
    // trigger some animation.
  }

  public void StopVibrate()
  {
    // send data to certain port....
    isTriggered = false;
    Debug.Log("Send stop command to PORT:1000");
  }


  // Update is called once per frame
  void Update()
  {
    if (isTriggered)
    {
      if (Time.time <= endTime)
      {
        Vibrate();
      }
      else
      {
        StopVibrate();
      }
    }



  }

  void OnTriggerEnter(Collider other)
  {

    StartVibrate();
  }
}
