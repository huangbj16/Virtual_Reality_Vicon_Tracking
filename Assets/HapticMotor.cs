using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HapticMotor : MonoBehaviour
{

  public bool isTriggered = false;

  private float endTime;

  public int intensity = 1;

  private VisualEffect visualEffect;

  public int flag = 0;

  void Start()
  {
    visualEffect = GetComponent<VisualEffect>();
  }

  public void StartVibrate()
  {
    isTriggered = true;
    visualEffect.Reinit();
    // send data to certain port....
    // visualEffect.Play();
    visualEffect.enabled = true;
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
    // visualEffect.Stop();
    visualEffect.enabled = false;
    Debug.Log("Send stop command to PORT:1000");
  }


  // Update is called once per frame
  void Update()
  {
    if (isTriggered)
    {
      Vibrate();
    }
  }

  void OnTriggerEnter(Collider other)
  {
    StartVibrate();
  }

  void OnTriggerExit(Collider other)
  {
    StopVibrate();
  }
}
