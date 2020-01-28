using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
  [SerializeField] int healthMax = 1000;
  [SerializeField] int health;
  void Start()
  {
    health = healthMax;
  }
  void Update()
  {
    if (health > healthMax)
      health = healthMax;
  }
  public int GetHealth() { return this.health; }
  public float GetHealthPrecentage() { return this.health / this.healthMax; }
  public void AddHealth(int addedHealth) { this.health += addedHealth; }
  public void ReduceHealth(int reducedHealth) { this.health -= reducedHealth; }
  //Player-character methods:
  public bool HealthAtMax() { return health == healthMax; }
  public void ResetHealth() { this.health = healthMax; }
}
