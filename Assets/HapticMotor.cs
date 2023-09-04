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

  public ShapeGenerator shapeGenerator;


  void Start()
  {

    shapeGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShapeGenerator>();
    visualEffect = GetComponent<VisualEffect>();
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
    string commandString = DictionaryToString(command) + "\n";
    Debug.Log(commandString);
    sender.SendData(commandString);
    collusionData = new CollusionData(motor_id, other);
  }

  public void Vibrate()
  {
    // trigger some animation.

  }

  public void StopVibrate(string other)
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
    string commandString = DictionaryToString(command) + "\n";
    Debug.Log(commandString);
    sender.SendData(commandString);
    sender.SendData(commandString);
    collusionData.CalculateCollusionDuration();
    Debug.Log(collusionData.actutorId + ": " + other);
    shapeGenerator.currentUserData.TriggerAlert(collusionData);
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
    Debug.Log("enter " + other.name + ", " + other.transform.parent.parent.name);

    if (other.transform.parent.GetComponent<ShapeObject>() != null)
    {
      Debug.Log("collision starts with " + other.transform.parent.parent.name);
      StartVibrate(other.transform.parent.parent.name);
    }
  }

  void OnTriggerExit(Collider other)
  {
    Debug.Log("exit");
    if (other.transform.parent.GetComponent<ShapeObject>() != null)
    {

      Debug.Log("collision ends with " + other.transform.parent.parent.name);
      StopVibrate(other.transform.parent.parent.name);
    }

  }
}
