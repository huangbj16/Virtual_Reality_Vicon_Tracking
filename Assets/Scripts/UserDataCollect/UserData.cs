using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UserData
{
  public string userId;
  public string date;
  public string gameName;
  public string mode; // PRACTICE, PLAYGROUND, VISUAL_ONLY, HAPTIC
  public int numberOfRound;
  public int numberOfShape;
  public int roundNumber;

  public int numberOfAlert;

  public List<AlertData> alertData;

  public UserData(string userId, string date, string gameName, ShapeGeneratorConfig gameConfig, int roundNumber)
  {
    this.userId = userId;
    this.date = date;
    this.gameName = gameName;
    this.mode = gameConfig.mode;
    this.numberOfRound = gameConfig.numberOfRound;
    this.numberOfShape = gameConfig.numberOfShape;
    this.roundNumber = roundNumber;
    this.numberOfAlert = 0;
    this.alertData = new List<AlertData>();
  }

  public void TriggerAlert(int actutorId, float currentTimestamp)
  {
    this.alertData.Add(new AlertData(actutorId, currentTimestamp));
  }

}


[System.Serializable]
public class AlertData
{
    public int actutorId;
    public float timestamp;
    public float duration;

  public AlertData(int actutorId)
  {
        this.actutorId = actutorId;
        this.timestamp = Time.time;
  }
    
  public void CalculateAlertDuration()
    {
        this.duration = Time.time - this.timestamp;
    } 
}