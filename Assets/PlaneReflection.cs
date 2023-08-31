using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneReflection : MonoBehaviour
{
  [SerializeField] private Camera reflectionCamera;
  [SerializeField] private RenderTexture reflectionRenderTexture;

  private void LateUpdate()
  {
    reflectionCamera.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 180, Camera.main.transform.eulerAngles.z);
  }

}
