using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class ShapeGenerator : MonoBehaviour
{
  public float timeEscape = 1.0f;
  public float movementSpeed = 1.0f;
  public float distance = 5f;
  public static GameObject[] shapePrefabs;
  public float previousTimeStamp = 0.0f;
  public ShapeGeneratorConfig currentConfig;
  private List<GameObject> shapeObjects;
  private List<int> shapeRandomIndexs;
  GameObject destinationObject;

  public UserData currentUserData;

  // game info
  public int currentShapeCount = 0;
  // this will control the game
  public bool pause = true;
  public string username = "user";
  public int roundNumber = 0;

  // Start is called before the first frame update
  void Start()
  {
    destinationObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    ResetDestination();
    shapePrefabs = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    shapeObjects = new List<GameObject>();
    shapeRandomIndexs = new List<int>();
    // set mode to practice by default
    SetMode(ShapeGeneratorConstants.PRACTICE_MODE);
    currentUserData = new UserData(username, getDateTime(), "OhShape", currentConfig, roundNumber);
    }

  // Update is called once per frame
  void Update()
  {
    // generate the wall
    if (!pause && currentShapeCount < currentConfig.numberOfShapes)
    {
      if (Time.time - previousTimeStamp > timeEscape)
      {
        GenerateShape(movementSpeed, shapePrefabs, shapeRandomIndexs, shapeObjects);
        currentShapeCount += 1;
        previousTimeStamp = Time.time;
      }
    }
    else if (currentShapeCount == currentConfig.numberOfShapes && pause == false)
    {
      pause = true;
    }
  }

  void GenerateShape(float speed, GameObject[] shapePrefabs, List<int> shapeRandomIndexs, List<GameObject> shapeObjects)
  {
    int shapeIndex = shapeRandomIndexs[currentShapeCount];
    GameObject shapeToBuild = shapePrefabs[shapeIndex];
    GameObject newShape = Instantiate(shapeToBuild, Vector3.zero, Quaternion.identity) as GameObject;
    newShape.name = "shape" + currentShapeCount.ToString();
    newShape.transform.SetParent(transform);
    newShape.transform.localPosition = Vector3.zero;
    newShape.GetComponentInChildren<ShapeObject>().Initialize(Vector3.zero, Quaternion.identity, speed);
    shapeObjects.Add(newShape);
  }

  public void ResetDirection(Vector3 newDirection)
  {
    transform.rotation = Quaternion.Euler(newDirection.x, newDirection.y, newDirection.z);
    ResetDestination();
    foreach (GameObject shape in shapeObjects)
    {
      shape.GetComponentInChildren<ShapeObject>().Reset(shape.transform.localPosition, shape.transform.localRotation, movementSpeed);
    }
  }

  void ResetDestination()
  {
    destinationObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    destinationObject.transform.localPosition = transform.forward * distance;
    destinationObject.transform.localRotation = Quaternion.identity;
  }

  public void SetMode(string mode)
  {
    shapeRandomIndexs.Clear();
    string configPath = System.IO.Path.Combine(Application.dataPath, ShapeGeneratorConstants.CONFIG_LOAD_PATH + mode + ".json");
    string configString = System.IO.File.ReadAllText(configPath);
    currentConfig = JsonUtility.FromJson<ShapeGeneratorConfig>(configString);
    //TODO: trigger directional light on / off
  }

  public void ResetGame()
  {
    currentShapeCount = 0;
    int shapePrefabLength = shapePrefabs.Length;
    int[] rangeToArray = Enumerable.Range(0, shapePrefabLength).ToArray();
    for (int i = 0; i < currentConfig.numberOfShapes / shapePrefabLength; i++)
    {
      shapeRandomIndexs.AddRange(new List<int>(rangeToArray));
    }
    Shuffle(shapeRandomIndexs);
    // init userData HARD-CODE
    currentUserData = new UserData(username, getDateTime(), "OhShape", currentConfig, roundNumber);
  }

  public void ExportUserData()
  {
    string exportFolderPath = System.IO.Path.Combine(Application.dataPath, ShapeGeneratorConstants.USER_DATA_EXPORT_PATH + username);
    if (!Directory.Exists(exportFolderPath))
    {
      Directory.CreateDirectory(exportFolderPath);
    }


    string jsonData = JsonUtility.ToJson(currentUserData);
    string filePath = exportFolderPath + "/" + getDateTime() + ".json";
    File.WriteAllText(filePath, jsonData);

  }

  public void StartGame()
  {
    ResetGame();
    pause = false;
  }


  // public void PauseGame()
  // {
  //   pause = true;
  // }


  List<int> Shuffle(List<int> a)
  {
    // Loops through array
    for (int i = a.Count - 1; i > 0; i--)
    {
      // Randomize a number between 0 and i (so that the range decreases each time)
      int rnd = Random.Range(0, i);

      // Save the value of the current i, otherwise it'll overright when we swap the values
      int temp = a[i];

      // Swap the new and old values
      a[i] = a[rnd];
      a[rnd] = temp;
    }
    return a;
  }

  string getDateTime()
  {
    string theTime = System.DateTime.Now.ToString("hh-mm-ss");
    string theDate = System.DateTime.Now.ToString("MM-dd-yyyy");
    return theDate + '-' + theTime;
  }
}
