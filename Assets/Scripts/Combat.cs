using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
  Animator animator;
  [SerializeField] bool isHitstunned = false;
  [SerializeField] float attackInitDelay = 0.0001f;
  bool canInitiate = true;
  [SerializeField] float hitstunTimerTest = 5f; //In seconds!!!!
  [SerializeField] float hitstunTimerLight = 300f;
  [SerializeField] float hitstunTimerMedium = 300f;
  [SerializeField] float hitstunTimerHeavy = 300f;
  [SerializeField] float hitstunTimerAir = 300f;
  private float currentHitstunTimer;
  bool canInvokeHitstunReset, wasHit;

  //add property descriptions for inspector

  public enum AttackType
  {
    none,
    light,
    medium,
    heavy,
    air
  }

  void Start()
  {
    animator = GetComponent<Animator>();
    currentHitstunTimer = 0f;
    canInvokeHitstunReset = true;
    wasHit = false;
  }
  void Update()
  {
    if (isHitstunned && canInvokeHitstunReset)
    {
      //add set stun animation
      canInvokeHitstunReset = false;
      Invoke("ResetHitstun", currentHitstunTimer);
    }
    if (isHitstunned && !canInvokeHitstunReset && wasHit)
    {
      //Stun anim again? Maybe put in function?
      CancelInvoke("ResetHitstun");
      Invoke("ResetHitstun", currentHitstunTimer);
    }
    // if (!isHitstunned)
    //   wasHit = false;
    // if (this.gameObject.CompareTag("Enemy"))
    //   Debug.Log(isHitstunned);
  }
  public void InitiatePunchSystem(Collider2D enemyCollider) //Fix so punches don't go off automatically.
  {
    if (canInitiate)
    {
      canInitiate = false;
      Combat enemyCombatComp = enemyCollider.transform.parent.gameObject.GetComponent<Combat>(); // Handle errors here if no combat component ound or if no parent and such
      enemyCombatComp.SetWasHit(true);
      enemyCombatComp.SetCurrentHitstun(GetHitstunByAttackType(GetAttackType()));
      if (!enemyCombatComp.GetIsHitsunned())
      {
        enemyCombatComp.SetIsHitstunned(true);
        // return; //before return - add a bit of stun. unless it is already part of the uppercut animation
      }
      else if (enemyCombatComp.GetIsHitsunned() && (Input.GetButtonDown("Punch") || Input.GetButtonDown("Heavy Punch") || Input.GetButtonDown("Uppercut")))
      {
        if (Input.GetButtonDown("Punch")) animator.SetTrigger("isPunching");
        else if (Input.GetButtonDown("Heavy Punch")) animator.SetTrigger("isHardPunching");
        else if (Input.GetButtonDown("Uppercut")) animator.SetTrigger("isUppercutting");
        // Invoke("ResetAttack", attackInitDelay); //Maybe just caninit = true? without invoke?
        // return;
      }
      // Invoke("ResetAttack", attackInitDelay); //Maybe just caninit = true? without invoke?
      canInitiate = true;
    }
  }
  private void OnTriggerExit2D(Collider2D enemyCollider) {
    if (enemyCollider.gameObject.CompareTag("Consumable") && enemyCollider.gameObject.layer == LayerMask.NameToLayer("Consumables"))
      return; //only temporary, need to find solution for general objects if even needed
    if (!enemyCollider.transform.parent.gameObject.CompareTag("Enemy"))
      return;
    Combat enemyCombatComp = enemyCollider.transform.parent.gameObject.GetComponent<Combat>();
    enemyCombatComp.SetWasHit(false);
  }
  
  private void ResetAttack() { canInitiate = true; }
  // private bool isHitStunned(GameObject enemy) { return enemy.GetComponent<Combat>().isHitstunned; }
  private void ResetHitstun()
  {
    isHitstunned = false;
    canInvokeHitstunReset = true;
    //turn off stun animation
  }
  public void SetCurrentHitstun(float newCurrentHitstun) { this.currentHitstunTimer = newCurrentHitstun; }
  public void SetIsHitstunned(bool newHitstunStatus) { this.isHitstunned = newHitstunStatus; }
  public bool GetIsHitsunned() { return this.isHitstunned; }
  public void SetWasHit(bool newWasHitStatus) { this.wasHit = newWasHitStatus; } 
  private float GetHitstunByAttackType(AttackType attackType)
  {
    if (attackType == AttackType.light)
      return this.hitstunTimerLight;
    else if (attackType == AttackType.medium)
      return this.hitstunTimerMedium;
    else if (attackType == AttackType.heavy)
      return this.hitstunTimerHeavy;
    // else if (attackType == AttackType.air)
    //   return this.hitstunTimerAir;
    return 0f;
  }
  public AttackType GetAttackType() //maybe leave public so enemy can check patterns
  {
    if (Input.GetButton("Punch"))
      return AttackType.light;
    else if (Input.GetButton("Heavy Punch"))
      return AttackType.medium;
    else if (Input.GetButton("Uppercut"))
      return AttackType.heavy;
    // if (Input.GetButton("Punch"))
    //   return AttackType.light; //Handle jumping and then punching
    return AttackType.none; //use for error handling
  }
}