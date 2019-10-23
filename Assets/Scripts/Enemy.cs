using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        initializing,
        idle,
        detectedPlayer,
        chasingPlayer,
        attacking,
        runningAway,
        blocking,
        dead
    }

    EnemyState currentState;
    GameObject playerRef;

    void Start()
    {
        currentState = EnemyState.initializing;
    }
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.initializing:
                playerRef = GameObject.FindGameObjectWithTag("Player");
                currentState = EnemyState.idle;
                break;
            case EnemyState.idle:
                //
                break;
            case EnemyState.detectedPlayer:
                //
                break;
            case EnemyState.chasingPlayer:
                //
                break;
            case EnemyState.attacking:
                //
                break;
            case EnemyState.runningAway:
                //
                break;
            case EnemyState.blocking:
                //
                break;
            case EnemyState.dead:
                //
                break;
        }
    }

    private void Idle()
    {
        
    }
    private void DetectedPlayer()
    { }
    private void ChasingPlayer()
    { }
    private void Attacking()
    { }
    private void RunningAway()
    { }
    private void Blocking()
    { }
    private void Dead()
    { }
}