using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
  Player playerScript;
  Health playerHealth, currentEnemyHealth;
  [SerializeField] Slider playerHealthbar, currentEnemyHealthbar;
  [SerializeField] TextMeshProUGUI playerScore;
  void Start()
  {
    playerScript = FindObjectOfType<Player>();
    //Handle error
    playerHealth = playerScript.GetComponent<Health>();
    //Handle error
    //Handle healthbar, score etc. errors

    playerHealthbar.value = playerHealth.GetHealthPrecentage();
  }

  void Update()
  {
    playerHealthbar.value = playerHealth.GetHealthPrecentage();
    playerScore.text = "SCORE: " + playerScript.GetPlayerScore().ToString("D5");
  }
}
