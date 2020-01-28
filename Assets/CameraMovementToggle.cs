using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementToggle : MonoBehaviour
{
  CameraFollow gameCameraScript;
  [SerializeField] bool togglerState;
  void Start()
  {
    gameCameraScript = FindObjectOfType<Camera>().gameObject.GetComponent<CameraFollow>();
    //Handle Error
  }
  private void OnTriggerEnter2D(Collider2D otherCollider)
  {
    if (otherCollider.transform.parent.gameObject.CompareTag("Player"))
      gameCameraScript.SetStayInPlace(false);
  }
}
