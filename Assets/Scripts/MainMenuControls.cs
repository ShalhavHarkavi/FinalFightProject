using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuControls : MonoBehaviour
{
  SceneLoader sceneLoader;
  Animator animator;
  TextMeshProUGUI startText;
  Button [] buttons;
  AudioSource sound;
  [Header("Sound Clips")]
  [SerializeField] AudioClip MenuStart;
  [SerializeField] AudioClip MenuMove;
  [SerializeField] AudioClip MenuSelect;
  [SerializeField] AudioClip MenuMusic;
  bool isPreMenuOptions, switchedToOptions;
  void Start()
  {
    isPreMenuOptions = true;
    switchedToOptions = false;
    sceneLoader = FindObjectOfType<SceneLoader>();
    if (!sceneLoader)
    {
      Debug.LogError("No scene loader found");
      Application.Quit();
    }
    startText = GameObject.Find("PressStart").GetComponent<TextMeshProUGUI>();
    //Handle error
    //if startText.text is not press start => error
    animator = startText.GetComponent<Animator>();
    //Handle error
    buttons = FindObjectsOfType<Button>();
    //Handle error
    sound = GetComponent<AudioSource>();
    //Handle error
    ToggleButtons(false);
    // DontDestroyOnLoad(this.gameObject);
    // if (!sceneLoader.GetCurrentScene().Equals(sceneLoader.GetMainMenuName()) ||
    //     !sceneLoader.GetCurrentScene().Equals(sceneLoader.GetOptionsMenuName()))
    //   Destroy(this.gameObject);
  }
  void Update()
  {
    if (Input.GetButtonDown("Start") && isPreMenuOptions)
    {
      animator.SetTrigger("startPressed");
      sound.loop = false;
      sound.PlayOneShot(MenuStart);
      switchedToOptions = true;
    }
    if (!isPreMenuOptions && switchedToOptions)
    {
      switchedToOptions = false;
      startText.enabled = false;
      ToggleButtons(true);
      sound.loop = true;
      sound.PlayOneShot(MenuMusic);
    }
  }
  public void SetIsPreMenuOptions(int state)
  {
    if (state == 1)
      isPreMenuOptions = true;
    else
      isPreMenuOptions = false;
  }
  private void ToggleButtons(bool status)
  {
    foreach(Button button in buttons)
      button.gameObject.SetActive(status);
  }
}
