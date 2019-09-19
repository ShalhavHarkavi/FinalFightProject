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

    float xMin, xMax, yMin, yMax;
    bool canMove, isAttacking;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Camera gameCamera;

    void Start()
    {
        canMove = true;
        isAttacking = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gameCamera = Camera.main;
        SetUpMoveBoundaries();
    }
    void Update()
    {
        Move();
        Attack();
        if (isAttacking == true)
            canMove = false;
        else
            canMove = true;
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
            StartCoroutine("AttackCoroutine");
            
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("isPunching");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        isAttacking = false;
    }
}