using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

[Serializable]
public class HapticMotor : MonoBehaviour
{

  public bool isTriggered = false;

  private float endTime;

  public int intensity = 1;

  private VisualEffect visualEffect;

  public int flag = 0;

  private GameObject senderObject;
  private TcpSender sender;
  public Dictionary<string, int> command;

  public int motor_id = 0;

  public CollusionData collusionData;

  void Start()
  {
    visualEffect = GetComponent<VisualEffect>();
    senderObject = GameObject.Find("TCPSenderObject");
    sender = senderObject.GetComponent<TcpSender>();
    command = new Dictionary<string, int>
        {
            { "addr", motor_id },
            { "mode", 0 },
            { "duty", 3 },
            { "freq", 2 },
            { "wave", 1 }
        };
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

  public void StartVibrate(string other)
  {
    isTriggered = true;
    // trigger visual effect if exists
    if (visualEffect != null)
    {
      visualEffect.Reinit();
      // visualEffect.Play();
      visualEffect.enabled = true;
    }
    // send TCP command
    Debug.Log("Send start command to PORT:9051");
    command["mode"] = 1;
    string commandString = DictionaryToString(command);
    Debug.Log(commandString);
    sender.SendData(commandString);
    collusionData = new CollusionData(motor_id, other);
  }

  public void Vibrate()
  {
    // trigger some animation.

  }

  public void StopVibrate()
  {
    // stop visual effect if exists
    if (visualEffect != null)
    {
      isTriggered = false;
      // visualEffect.Stop();
      visualEffect.enabled = false;
    }
    //send TCP command
    Debug.Log("Send stop command to PORT:9051");
    command["mode"] = 0;
    string commandString = DictionaryToString(command);
    Debug.Log(commandString);
    sender.SendData(commandString);
    collusionData.CalculateCollusionDuration();
    // add event handler to trigger ohshape generator
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
    Debug.Log("collision starts with " + other.name);
    StartVibrate(other.name);
  }

  void OnTriggerExit(Collider other)
  {
    Debug.Log("collision ends with " + other.name);
    StopVibrate();
  }
}
