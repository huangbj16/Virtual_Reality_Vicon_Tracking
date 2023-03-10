using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeObject : MonoBehaviour
{
  public Vector3 destination;
  public float movementSpeed;

  private Vector3 direction;

  private bool initialized = false;

  public void initialize(Vector3 dest, float speed)
  {
    transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
    destination = dest;
    movementSpeed = speed;
    direction = (destination - transform.position).normalized;
    initialized = true;
  }

  public void move()
  {
    if ((destination - transform.position).magnitude < 0.05f)
    {
      Destroy(gameObject, 1f);
    }

    transform.position += direction * movementSpeed * Time.deltaTime;

  }


  void Update()
  {
    if (initialized) move();
  }
}
