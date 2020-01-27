using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStartAnim : MonoBehaviour
{
  MainMenuControls mainMenuControls;

  void Start()
  {
    mainMenuControls = FindObjectOfType<MainMenuControls>();
    if (!mainMenuControls)
    {
      Debug.LogError("No Main Menu Controller found.");
      Application.Quit();
    }
  }
  public void SetIsPreMenuOptions(int state)
  {
    mainMenuControls.SetIsPreMenuOptions(state);
  }
}
