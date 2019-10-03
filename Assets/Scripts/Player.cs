using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float xAxisMoveSpeed = 5f;
    [SerializeField] float yAxisMoveSpeed = 3f;
    [Range(0,1)] [SerializeField] float yWorldPointMax = 1f;
    [SerializeField] float xPadding = 0f;
    [SerializeField] float yPadding = 0f;
    [SerializeField] float punchTimerMax = 1.5f;
    //Set up different menus for accessibility!

    float xMin, xMax, yMin, yMax, punchTimer;
    bool canMove, doCountPunchTimer, isJumping, isNearItem;
    int movingState, punchCounter;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Camera gameCamera;

    void Start()
    {
        canMove = true;
        movingState = 1;
        punchCounter = 0;
        punchTimer = punchTimerMax;
        isJumping = false;
        isNearItem = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gameCamera = Camera.main;
        SetUpMoveBoundaries();
    }
    void Update()
    {
        Move();
        Attack();
        if (movingState == 1)
            canMove = true;
        if (movingState == 0)
            canMove = false;
        if (doCountPunchTimer)
            punchTimer -= Time.deltaTime;
        if (!doCountPunchTimer && canMove)
            punchCounter = 0;
    }
    private void SetUpMoveBoundaries()
    {
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, yWorldPointMax, 0)).y;
    }
    private void Move()
    {
        if (canMove)
        {
            float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * xAxisMoveSpeed;
            var newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
            float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * yAxisMoveSpeed;
            var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
            transform.position = new Vector2(newXpos, newYpos);
            if (deltaX != 0)
                transform.localScale = new Vector2(Mathf.Sign(deltaX), 1f);
            animator.SetBool("isWalking", (deltaX != 0 || deltaY != 0));
        }
    }
    private void Attack()
    {
        if (Input.GetButtonDown("Punch"))
        {
            if (isNearItem)
                PickUpItem();
            else
                animator.SetTrigger("isPunching"); //TEMP: InitiatePunchSystem();
        }
    }
    private void OnTriggerEnter2D(Collider2D enemyCollider)
    {
        Debug.Log("Hit");
        InitiatePunchSystem();
    }
    private void PickUpItem()
    {
        throw new NotImplementedException();
    }
    private void InitiatePunchSystem() //Make sure second punch and onwards are only triggered on hit. Else, reg punch.
    {
        doCountPunchTimer = true;
        if (punchCounter > 3 || punchTimer <= 0f)
        {
            doCountPunchTimer = false;
            punchTimer = punchTimerMax;
            punchCounter = 0;
            return; //before return - add a bit of stun. unless it is already part of the uppercut animation
        }
        if (punchCounter == 0 || punchCounter == 1)
        {
            punchCounter++;
            animator.SetTrigger("isPunching");
            punchTimer = punchTimerMax;
        }
        else if (punchCounter == 2)
        {
            punchCounter++;
            animator.SetTrigger("isHardPunching");
            punchTimer = punchTimerMax;
        }
        else if (punchCounter == 3)
        {
            punchCounter++;
            animator.SetTrigger("isUppercutting");
            punchTimer = punchTimerMax;
        }
        
    }
    private void AttackAndWalkState(int state) { movingState = state; } //rename
}