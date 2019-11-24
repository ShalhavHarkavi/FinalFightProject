using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int playerHealthMax = 1000;
    [SerializeField] int playerPointsMax = 50000;
    [SerializeField] int playerLives = 3; //handle game over state - reset level if lives == 0
    [SerializeField] float xAxisMoveSpeed = 5f;
    [SerializeField] float yAxisMoveSpeed = 3f;
    [Range(0,1)] [SerializeField] float yWorldPointMax = 1f;
    [SerializeField] float xPadding = 0f;
    [SerializeField] float yPadding = 0f;
    [SerializeField] float punchTimerMax = 0.1f; //maybe remove?
    [SerializeField] List<string> animationTriggerNames; //maybe remove?
    [SerializeField] float jumpSpeed = 10f; //maybe add range?
    //Set up headers for accessibility!

    float xMin, xMax, yMin, yMax, punchTimer;
    bool canMove, canJump, doCountPunchTimer, isJumping, isNearItem;
    int playerHealth, playerPoints, movingState, punchCounter;
    Collider2D consumableCollider = null;
    private float timeBtwAttack;
    [SerializeField] public float startTimeBtwAttack = 0.5f;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D playerRigidbody;
    Camera gameCamera;
    Combat combat;

    void Start()
    {
        playerPoints = 0;
        playerHealth = playerHealthMax;
        canMove = true;
        canJump = true;
        movingState = 1;
        punchCounter = 0;
        punchTimer = punchTimerMax;
        isJumping = false;
        isNearItem = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        combat = GetComponent<Combat>();
        gameCamera = Camera.main;
        SetUpMoveBoundaries();
    }
    void Update()
    {
        Attack();
        Move();
        Jump();
        if (movingState == 1)
            canMove = true;
        if (movingState == 0)
            canMove = false;
        PointsToLives();
        LivesToHealth();
        if (playerHealth > playerHealthMax)
            playerHealth = playerHealthMax;
        // HandleGameOver(); //maybe do this in LivesToHealth()?
        // Debug.Log(); //Test message
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
    private void Jump()
    {
        if (Input.GetButtonDown("Jump")) //&& canJump
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpSpeed);
            //playerRigidbody.AddForce(Vector2.up, ForceMode2D.Force);
        }
    }
    private void Attack()
    {
        if (Input.GetButtonDown("Punch"))
        {
            if (isNearItem && consumableCollider != null)
                PickUpItem(consumableCollider);
            else
                animator.SetTrigger("isPunching");
        }
        else if (Input.GetButtonDown("Heavy Punch"))
            animator.SetTrigger("isHardPunching");
        else if (Input.GetButtonDown("Uppercut"))
            animator.SetTrigger("isUppercutting");
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Consumable"))
        {
            isNearItem = true;
            consumableCollider = collider;
        }
        else
        {
            animator.SetTrigger("didHit");
            combat.InitiatePunchSystem(collider);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Consumable"))
        {
            isNearItem = false;
            consumableCollider = null;
        }
    }
    private void PickUpItem(Collider2D consumable)
    {
        Consumable consumableComponent = consumable.gameObject.GetComponent<Consumable>();
        animator.SetTrigger("pickUp");
        Consumable.ItemType consumableType = consumableComponent.getConsumableType();
        if (consumableType == Consumable.ItemType.health)
        {
            if (playerHealth == playerHealthMax)
                playerPoints += consumableComponent.getConsumableValue();
            else
                playerHealth += consumableComponent.getConsumableValue();
        }
        else if (consumableType == Consumable.ItemType.points)
            playerPoints += consumableComponent.getConsumableValue();
        else if (consumableType == Consumable.ItemType.lives)
            playerLives += consumableComponent.getConsumableValue();
        Destroy(consumableCollider.gameObject);
    }
    private void PointsToLives()
    {
        if (playerPoints >= playerPointsMax)
        {
            playerPoints = 0;
            playerLives++;
            //add 1up sound effect and message?
        }
    }
    private void LivesToHealth()
    {
        if (playerHealth <= 0)
        {
            playerHealth = playerHealthMax;
            playerLives--;
            //maybe add effects?
        }
    }
    private void AttackAndWalkState(int state) { movingState = state; }
}