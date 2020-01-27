
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;
  public float smoothSpeed = 0.125f;
  
  void LateUpdate()
  {
    transform.position = new Vector3(target.position.x, 0, -10);
  }
}
