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

  public int numberOfCollusion;

  public Dictionary<string, List<CollusionData>> collusionData;

  public UserData(string userId, string date, string gameName, ShapeGeneratorConfig gameConfig, int roundNumber)
  {
    this.userId = userId;
    this.date = date;
    this.gameName = gameName;
    this.mode = gameConfig.mode;
    this.numberOfRound = gameConfig.numberOfRound;
    this.numberOfShape = gameConfig.numberOfShape;
    this.roundNumber = roundNumber;
    this.numberOfCollusion = 0;
    this.collusionData = new Dictionary<string, List<CollusionData>>();
  }

  public void TriggerAlert(int actutorId, string other)
  {
    if (!this.collusionData.ContainsKey(other))
    {
      this.collusionData[other] = new List<CollusionData>();
    }
    this.collusionData[other].Add(new CollusionData(actutorId, other));

  }

}


[System.Serializable]
public class CollusionData
{
  public int actutorId;
  public float timestamp;
  public float duration;

  public string other;

  public CollusionData(int actutorId, string other)
  {
    this.actutorId = actutorId;
    this.timestamp = Time.time;
    this.other = other;
  }

  public void CalculateCollusionDuration()
  {
    this.duration = Time.time - this.timestamp;
  }
}