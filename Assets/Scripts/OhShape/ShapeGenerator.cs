using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
  public float timeEscape = 1.0f;
  public float movementSpeed = 1.0f;
  public float distance = 5f;
  public Vector3 shapeDestination;

  public static GameObject[] shapePrefabs;

  public float previousTimeStamp = 0.0f;

  private List<GameObject> shapeObjects;

  // Start is called before the first frame update
  void Start()
  {

    shapeDestination = transform.position + transform.forward * distance;
    GameObject destinationObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    destinationObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    destinationObject.transform.localPosition = transform.forward * distance;
    destinationObject.transform.localRotation = Quaternion.identity;

    shapePrefabs = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    shapeObjects = new List<GameObject>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - previousTimeStamp > timeEscape)
    {
      GenerateShape(transform.position, transform.rotation, shapeDestination, movementSpeed, shapePrefabs, shapeObjects);
      previousTimeStamp = Time.time;
    }

  }

  void GenerateShape(Vector3 initialPosition, Quaternion initialRotation, Vector3 destination, float speed, GameObject[] shapePrefabs, List<GameObject> shapeObjects)
  {
    GameObject shapeToBuild = shapePrefabs[Random.Range(0, shapePrefabs.Length)];
    GameObject newShape = Instantiate(shapeToBuild, initialPosition, initialRotation) as GameObject;
    newShape.GetComponent<ShapeObject>().initialize(destination, speed);
    // ShapeObject newShape = new ShapeObject(initialPosition, initialRotation, destination, speed);
    shapeObjects.Add(newShape);
  }
}
