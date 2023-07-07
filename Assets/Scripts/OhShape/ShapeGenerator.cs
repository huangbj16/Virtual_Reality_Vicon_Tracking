using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

  // game info
  public int currentShapeCount = 0;
  // this will control the game
  public bool pause = true;

  // Start is called before the first frame update
  void Start()
  {
    destinationObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    ResetDestination();
    shapePrefabs = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    shapeObjects = new List<GameObject>();
    shapeRandomIndexs = new List<int>();
    // set mode to practice by default
    setMode(ShapeGeneratorConstants.PRACTICE_MODE);
  }

  // Update is called once per frame
  void Update()
  {
    // generate the wall
    if (!pause && currentShapeCount < currentConfig.numberOfShape)
    {
      if (Time.time - previousTimeStamp > timeEscape)
      {
        GenerateShape(movementSpeed, shapePrefabs, shapeRandomIndexs, shapeObjects);
        currentShapeCount += 1;
        previousTimeStamp = Time.time;
      }
    }
    else if (currentShapeCount == currentConfig.numberOfShape)
    {
      resetShapeGeneator();
    }
  }

  void GenerateShape(float speed, GameObject[] shapePrefabs, List<int> shapeRandomIndexs, List<GameObject> shapeObjects)
  {
    int shapeIndex = shapeRandomIndexs[currentShapeCount];
    GameObject shapeToBuild = shapePrefabs[shapeIndex];
    GameObject newShape = Instantiate(shapeToBuild, Vector3.zero, Quaternion.identity) as GameObject;
    newShape.transform.SetParent(transform);
    newShape.GetComponent<ShapeObject>().Initialize(Vector3.zero, Quaternion.identity, speed);
    shapeObjects.Add(newShape);
  }

  public void ResetDirection(Vector3 newDirection)
  {
    transform.rotation = Quaternion.Euler(newDirection.x, newDirection.y, newDirection.z);
    ResetDestination();
    foreach (GameObject shape in shapeObjects)
    {
      shape.GetComponent<ShapeObject>().Reset(shape.transform.localPosition, shape.transform.localRotation, movementSpeed);
    }
  }

  void ResetDestination()
  {
    destinationObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    destinationObject.transform.localPosition = transform.forward * distance;
    destinationObject.transform.localRotation = Quaternion.identity;
  }

  public void setMode(string mode)
  {
    string configPath = System.IO.Path.Combine(Application.dataPath, "Scripts/OhShape/ShapeGeneratorConfig/" + mode + ".json");
    string configString = System.IO.File.ReadAllText(configPath);
    currentConfig = JsonUtility.FromJson<ShapeGeneratorConfig>(configString);
    int shapePrefabLength = shapePrefabs.Length;

    int[] rangeToArray = Enumerable.Range(0, shapePrefabLength).ToArray();

    for (int i = 0; i < currentConfig.numberOfShape / shapePrefabLength; i++)
    {
      shapeRandomIndexs.AddRange(Shuffle(new List<int>(rangeToArray)));
    }
    shapeRandomIndexs.AddRange(Shuffle(new List<int>(rangeToArray)).Take(currentConfig.numberOfShape % shapePrefabLength));
  }

  public void resetShapeGeneator()
  {
    pause = true;
    currentShapeCount = 0;
    shapeRandomIndexs.Clear();
  }

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
}
