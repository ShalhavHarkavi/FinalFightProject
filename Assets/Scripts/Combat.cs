using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    int punchCounter;
    Animator animator;
    [SerializeField] List<string> animationTriggerNames;
    [SerializeField] bool isHitstunned = false;

    [SerializeField] float attackInitDelay = .5f;
    [SerializeField] public float maxattackInitDelay = 0.5f;
    bool canInitiate = true;

    void Start()
    {
        punchCounter = 0;
        animator = GetComponent<Animator>();

        attackInitDelay = maxattackInitDelay;
    }
    public void InitiatePunchSystem(Collider2D enemyCollider) //Fix so punches don't go off automatically.
    {
        if (canInitiate)
        {
            canInitiate = false;
            if (punchCounter >= animationTriggerNames.Count || !isHitStunned(enemyCollider.gameObject))
            {
                punchCounter = 0;
                return; //before return - add a bit of stun. unless it is already part of the uppercut animation
            }
            else if (isHitStunned(enemyCollider.gameObject))
            {
                animator.SetTrigger("isPunching");
                Invoke("ResetAttack", attackInitDelay);
                punchCounter++;
                return;   
            }
            
        }
    }
    private void ResetAttack() { canInitiate = true; }
    private bool isHitStunned(GameObject enemy) { return enemy.GetComponent<Combat>().isHitstunned; }
}