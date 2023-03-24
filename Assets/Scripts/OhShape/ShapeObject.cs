using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeObject : MonoBehaviour
{

  public Vector3 localPosition;

  public Vector3 localRotation;
  public float movementSpeed = 1.0f;

  private Vector3 direction;

  private bool initialized = false;

  public void Initialize(Vector3 localPosition, Quaternion localRotation, float speed)
  {
    Reset(localPosition, localRotation, speed);
    initialized = true;
    Destroy(gameObject, 10f);
  }

  public void Reset(Vector3 localPosition, Quaternion localRotation, float speed)
  {
    transform.localRotation = localRotation;
    transform.localPosition = localPosition;
    movementSpeed = speed;
    direction = Vector3.forward;
  }
  public void Move()
  {
    transform.localPosition += direction * movementSpeed * Time.deltaTime;

  }


  void Update()
  {
    if (initialized) Move();
  }
}
