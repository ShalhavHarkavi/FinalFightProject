using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  static int levelIndex = 0;
  string currentSceneName;
  [Header("Scene Names")]
  [SerializeField] string startScreenName = "MainMenu";
  [SerializeField] string optionsScreenName = "Options";
  [SerializeField] string gameOverName = "GameOver";
  [SerializeField] List<string> levelNames = new List<string>();
  void Start()
  {
    currentSceneName = SceneManager.GetActiveScene().name;
  }
  public void LoadMainMenu() { SceneManager.LoadScene(startScreenName); }
  public void LoadOptionsMenu() { SceneManager.LoadScene(optionsScreenName); }
  public void LoadGame() { SceneManager.LoadScene(levelNames[0]); }
  public void LoadNextLevel()
  {
    levelIndex++;
    if (levelIndex < levelNames.Count)
      SceneManager.LoadScene(levelNames[levelIndex]);
    else
    {
      Debug.Log("Level index is out of bounds.");
      Quit();
    }
  }
  public void ReloadLevel() { SceneManager.LoadScene(levelNames[levelIndex]); }
  public void LoadGameOver() { SceneManager.LoadScene(gameOverName); }
  public void Quit() { Application.Quit(); }
  public string GetCurrentScene() { return SceneManager.GetActiveScene().name; }
  public string GetMainMenuName() { return startScreenName; }
  public string GetOptionsMenuName() { return optionsScreenName; }
}
