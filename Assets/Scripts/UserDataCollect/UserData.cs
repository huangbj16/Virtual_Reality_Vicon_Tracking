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
  public int[] shapeIndexs;
  public int roundNumber;

  public int numberOfCollusions;

  public List<CollusionData> collusionData;

  public UserData(string userId, string date, string gameName, ShapeGeneratorConfig gameConfig, int[] shapeIndexs, int roundNumber)
  {
    this.userId = userId;
    this.date = date;
    this.gameName = gameName;
    this.mode = gameConfig.mode;
    this.numberOfRounds = gameConfig.numberOfRounds;
    this.numberOfShapes = gameConfig.numberOfShapes;
    this.shapeIndexs = shapeIndexs;
    this.roundNumber = roundNumber;
    this.numberOfCollusions = 0;
    this.collusionData = new List<CollusionData>();
  }

  public void TriggerAlert(CollusionData data)
  {
        Debug.Log("data other = "+data.other);
        this.collusionData.Add(data);

    numberOfCollusions += 1;

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