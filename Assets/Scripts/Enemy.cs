using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public enum EnemyState
  {
    initializing,
    searchingPlayer,
    chasingPlayer,
    attacking,
    runningAway,
    blocking,
    dead
  }

  EnemyState currentState;
  GameObject playerRef;
  Animator animator;
  Combat combat;
  Health health;

  bool detectedPlayer, inAttackRange;
  [SerializeField] float attackDistance = 2f; //Temp value, maybe change from distance to another collider with different tag
  [SerializeField] float giveUpChaseDistance = 10f; //Temp value
  [SerializeField] int healthRunAwayBar = 100;

  void Start()
  {
    detectedPlayer = false;
    inAttackRange = false;
    currentState = EnemyState.initializing;
    animator = GetComponent<Animator>();
    combat = GetComponent<Combat>();
    health = GetComponent<Health>(); //handle errors across all files if no component was gotten (I guess that's how you spell it?)
  }
  void Update()
  {
    if (!combat.GetIsHitsunned())
    {
      switch (currentState)
      {
        case EnemyState.initializing:
          playerRef = GameObject.FindGameObjectWithTag("Player");
          currentState = EnemyState.searchingPlayer;
          break;
        case EnemyState.searchingPlayer:
          SearchingPlayer();
          break;
        case EnemyState.chasingPlayer:
          ChasingPlayer();
          break;
        case EnemyState.attacking:
          Attacking();
          break;
        case EnemyState.runningAway:
          RunningAway();
          break;
        case EnemyState.blocking:
          //
          break;
        case EnemyState.dead:
          //
          break;
      }
      if (health.GetHealth() <= healthRunAwayBar)
      {
        if (health.GetHealth() > 0)
          currentState = EnemyState.runningAway;
        else
          currentState = EnemyState.dead;
      }
      this.transform.localScale = new Vector2(((playerRef.transform.position.x > this.transform.position.x) ? 1 : -1), 1);
    }
  }

private void OnTriggerEnter2D(Collider2D other)
{
  if (!other.gameObject.transform.parent)
    return;
  if (other.gameObject.transform.parent.CompareTag("Player"))
  {
    if (other.gameObject.CompareTag("detectCollider"))
      detectedPlayer = true;
    if (other.gameObject.CompareTag("attackCollider"))
      inAttackRange = true;
    if (other.gameObject.CompareTag("Shield"))
      Debug.Log("Enemy hit Player's shield!"); //Handle shielding here
  }
}
void OnTriggerExit2D(Collider2D other)
{
  if (!other.gameObject.transform.parent)
    return;
  if ((other.transform.parent.gameObject.CompareTag("Player") && other.gameObject.CompareTag("attackCollider")))
    inAttackRange = false;
}
  private void SearchingPlayer()
  {
    if (detectedPlayer)
      currentState = EnemyState.chasingPlayer;
  }
  private void ChasingPlayer()
  {
    // animator.SetBool("isWalking", true);
    float distanceFromPlayer = Vector2.Distance(playerRef.transform.position, this.transform.position);
    transform.position = Vector2.MoveTowards(transform.position, playerRef.transform.position, 5f * Time.deltaTime); //maybe change 5f to something else
    if (inAttackRange)
    {
      // animator.SetBool("isWalking", false);
      currentState = EnemyState.attacking;
    }
  }
  private void Attacking()
  {
    if (!inAttackRange)
      currentState = EnemyState.chasingPlayer;
  }
  private void RunningAway()
  {
    transform.position = Vector2.MoveTowards(transform.position, playerRef.transform.position, -5f * Time.deltaTime);
  }
  private void Blocking()
  {

  }
  private void SearchingConsumable()
  {
    GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
    if (consumables == null)
    {
      currentState = EnemyState.runningAway; //decide if blocking or running
      return;
    }
    List<Vector2> consumableLocations = new List<Vector2>();
    foreach (GameObject consumable in consumables)
      consumableLocations.Add(consumable.transform.position);
    
  }
  private void Dead()
  {
    //death animation
    Destroy(this);
  }
}