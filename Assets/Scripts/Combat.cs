using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    Animator animator;
    [SerializeField] bool isHitstunned = false;
    [SerializeField] float attackInitDelay = .5f;
    bool canInitiate = true;
    [SerializeField] float hitstunTimerMax = 20; //in frames, not seconds
    float hitstunTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        hitstunTimer = hitstunTimerMax;
    }
    void Update()
    {
        /* if (isHitstunned)
        {
            hitstunTimer -= 1;
            Debug.Log(hitstunTimer);
        }
        if (hitstunTimer <= 0)
        {
            isHitstunned = false;
            hitstunTimer = hitstunTimerMax;
        }*/
    }
    public void InitiatePunchSystem(Collider2D enemyCollider) //Fix so punches don't go off automatically.
    {
        if (canInitiate)
        {
            canInitiate = false;
            if (!isHitStunned(enemyCollider.gameObject))
            {
                enemyCollider.gameObject.GetComponent<Combat>().isHitstunned = true;
                return; //before return - add a bit of stun. unless it is already part of the uppercut animation
            }
            else if (isHitStunned(enemyCollider.gameObject) && Input.GetKeyDown("Punch"))
            {
                animator.SetTrigger("isPunching");
                Invoke("ResetAttack", attackInitDelay);
                return;
            }
            
        }
    }
    private void ResetAttack() { canInitiate = true; }
    private bool isHitStunned(GameObject enemy) { return enemy.GetComponent<Combat>().isHitstunned; }
}