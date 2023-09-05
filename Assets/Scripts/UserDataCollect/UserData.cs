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
  public int numberOfRounds;
  public int numberOfShapes;
  public int roundNumber;

  public int numberOfCollusions;

  public Dictionary<string, List<CollusionData>> collusionData;

  public List<CollusionData> collusions;

  public UserData(string userId, string date, string gameName, ShapeGeneratorConfig gameConfig, int roundNumber)
  {
    this.userId = userId;
    this.date = date;
    this.gameName = gameName;
    this.mode = gameConfig.mode;
    this.numberOfRounds = gameConfig.numberOfRounds;
    this.numberOfShapes = gameConfig.numberOfShapes;
    this.roundNumber = roundNumber;
    this.numberOfCollusions = 0;
    this.collusionData = new Dictionary<string, List<CollusionData>>();
    this.collusions = new List<CollusionData>();
  }

  public void TriggerAlert(CollusionData data)
  {
        Debug.Log("collusionData = "+this.collusionData);
        Debug.Log("data other = "+data.other);
    if (!this.collusionData.ContainsKey(data.other))
    {
      this.collusionData[data.other] = new List<CollusionData>();
    }
    this.collusionData[data.other].Add(data);

    numberOfCollusions += 1;

  }

  public void ToObject()
  {
    foreach (KeyValuePair<string, List<CollusionData>> pair in this.collusionData)
    {
      this.collusions.AddRange(pair.Value);
    }

  }

}


[System.Serializable]
public class CollusionData
{
  public int actutorId;
  public float timestamp;
  public float duration;
  public string other;
  public string detail;
  public CollusionData(int actutorId, string other)
  {
    this.actutorId = actutorId;
    this.timestamp = Time.time;
    this.other = other;
    this.detail = other;
  }

  public void CalculateCollusionDuration()
  {
    this.duration = Time.time - this.timestamp;
  }
}