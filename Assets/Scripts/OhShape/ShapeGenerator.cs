using System.Collections;
using System.Collections.Generic;
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
  GameObject destinationObject;

  // Start is called before the first frame update
  void Start()
  {
    destinationObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    ResetDestination();
    shapePrefabs = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    shapeObjects = new List<GameObject>();

    // set mode to practice by default
    setMode(ShapeGeneratorConstants.PRACTICE_MODE);
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - previousTimeStamp > timeEscape)
    {
      GenerateShape(movementSpeed, shapePrefabs, shapeObjects);
      previousTimeStamp = Time.time;
    }

  }

  void GenerateShape(float speed, GameObject[] shapePrefabs, List<GameObject> shapeObjects)
  {
    GameObject shapeToBuild = shapePrefabs[Random.Range(0, shapePrefabs.Length)];
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
  }
}
