
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform playerTransform;
  public float smoothSpeed = 0.125f;
  bool stayInPlace;

  void Start()
  {
    stayInPlace = true;
  }
  
  void LateUpdate()
  {
    // if (!stayInPlace)
      // transform.position = new Vector3(playerTransform.position.x, 0, -10);
    transform.position = new Vector3(Mathf.Clamp(playerTransform.position.x, 0,100), 0, -10);
    // if (stayInPlace && playerTransform.position.x > )
  }
  public void SetStayInPlace(bool state) { stayInPlace = state; }
}
