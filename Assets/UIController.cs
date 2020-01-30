using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
  Player playerScript;
  Health playerHealth, currentEnemyHealth;
  Combat playerCombat;
  [SerializeField] Slider playerHealthbar;
  [SerializeField] GameObject currentEnemyUI;
  [SerializeField] TextMeshProUGUI playerScore;
  void Start()
  {
    currentEnemyUI.SetActive(false);
    playerScript = FindObjectOfType<Player>();
    //Handle error
    playerHealth = playerScript.GetComponent<Health>();
    //Handle error
    playerCombat = playerScript.GetComponent<Combat>();
    //Handle error
    //Handle healthbar, score etc. errors

    playerHealthbar.value = playerHealth.GetHealthPrecentage();
  }

  void Update()
  {
    playerHealthbar.value = playerHealth.GetHealthPrecentage();
    playerScore.text = "SCORE: " + playerScript.GetPlayerScore().ToString("D5");
    if (playerCombat.GetIsInCombat())
    {
      currentEnemyHealth = playerCombat.GetCurrentEnemyRef().GetComponent<Health>();
      currentEnemyUI.SetActive(true);
      currentEnemyUI.GetComponentInChildren<Slider>().value = currentEnemyHealth.GetHealthPrecentage();
    }
    else
    {
      currentEnemyHealth = null;
      currentEnemyUI.SetActive(false);
    }
  }
}
