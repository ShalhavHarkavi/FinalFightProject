using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  // [SerializeField] int playerHealthMax = 1000;
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
  [SerializeField] float gravity = 1f; //Need to tweak
  //Set up headers (and descriptions) for accessibility!

  float xMin, xMax, yMin, yMax, punchTimer, jumpTimer;
  bool canMove, canJump, doCountPunchTimer, isJumping, isNearItem, isShielding;
  // int playerHealth;
  int playerPoints, movingState, jumpingState, punchCounter;
  GameObject shield = null;
  Collider2D consumableCollider = null;
  private float timeBtwAttack; //maybe remove? check usablity
  [SerializeField] public float startTimeBtwAttack = 0.5f;

  Animator animator;
  SpriteRenderer spriteRenderer;
  Rigidbody2D playerRigidbody;
  Camera gameCamera;
  Combat combat;
  Health health;

  void Start()
  {
    playerPoints = 0;
    // playerHealth = playerHealthMax;
    canMove = true;
    canJump = true;
    movingState = 1;
    jumpingState = 1;
    punchCounter = 0;
    jumpTimer = 0;
    punchTimer = punchTimerMax;
    isJumping = false;
    isNearItem = false;
    isShielding = false;
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    playerRigidbody = GetComponent<Rigidbody2D>();
    combat = GetComponent<Combat>();
    health = GetComponent<Health>();
    gameCamera = Camera.main;
    SetUpMoveBoundaries();
    shield = transform.Find("Cody Shield").gameObject; //If need to change to previous version: GameObject.Find("Cody Shield");
    if (!shield)
      Debug.LogError("NO SHIELD OBJECT CONNECTED TO PLAYER!");
    else
      shield.SetActive(false);
  }
  void Update()
  {
    Attack();
    Move();
    InitiateJump();
    isShielding = Input.GetButton("Shield");
    if (isShielding)
      shield.SetActive(true);
    else
      shield.SetActive(false);
    if (isJumping)
      jumpTimer += Time.deltaTime;
    if (!isJumping)
      jumpTimer = 0; //Maybe reset through different func?
    Jump(jumpTimer);
    if (movingState == 1)
      canMove = true;
    if (movingState == 0)
      canMove = false;
    if (jumpingState == 1)
      canJump = true;
    if (jumpingState == 0)
      canJump = false; //Maybe wrap state settings in a function?
    //Find better way to write this, maybe change isJumping to trigger and add bool isJumping? then if input.getbuttondown("punch") && isJumping, trigger specific air kick?
    PointsToLives();
    LivesToHealth();
    // if (playerHealth > playerHealthMax)
    //   playerHealth = playerHealthMax;
    
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
    if (canMove && !isShielding)
    {
      float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * xAxisMoveSpeed;
      float newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
      float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * yAxisMoveSpeed; //maybe restrict y movemvent to !isJumping?
      float newYpos = 0;
      if (!isJumping)
        newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
      transform.position = new Vector2(newXpos, newYpos);
      if (deltaX != 0 && !isJumping)
        transform.localScale = new Vector2(Mathf.Sign(deltaX), 1f);
      animator.SetBool("isWalking", (deltaX != 0 || deltaY != 0));
    }
  }
  private void InitiateJump()
  {
    if (Input.GetButtonDown("Jump") && canJump)
    {
      isJumping = true;
      animator.SetTrigger("isJumping");
    }
  }
  private void Jump(float jumpTimer)
  {
    if (isJumping)
    {
      //float xSpeed = xAxisMoveSpeed; //variable => 0 if no x movement
      float xSpeed = 0;
      transform.position += new Vector3(xSpeed, jumpSpeed * jumpTimer - 0.5f * gravity * Mathf.Pow(jumpTimer, 2));
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
    if (collider.gameObject.CompareTag("Consumable") && collider.gameObject.layer == LayerMask.NameToLayer("Consumables"))
    {
      isNearItem = true;
      consumableCollider = collider;
    }
    else if (collider.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
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
      if (health.HealthAtMax())
        playerPoints += consumableComponent.getConsumableValue();
      else
        health.AddHealth(consumableComponent.getConsumableValue());
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
    if (health.GetHealth() <= 0)
    {
      health.ResetHealth();
      playerLives--;
      //maybe add effects?
    }
  }
  private void AttackAndWalkState(int state) { movingState = state; }
  private void JumpState(int state) { jumpingState = state; }
}