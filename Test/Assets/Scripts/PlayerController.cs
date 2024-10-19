using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        IDLE,
        MOVING,
        DEAD,
        WINNER,
    }

    private Rigidbody rb;
    public PlayerState plState;
    private Vector3 direction;
    private LevelManager manager;

    public float magnitude;
    public Transform goalPos;

    private Animator animator;

    void Start()
    {

        rb = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();

        manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        direction = goalPos.position - transform.position;
        plState = PlayerState.MOVING;

        magnitude += Random.Range(0.001f, 0.01f);
    }

    void Update()
    {
        if (plState == PlayerState.MOVING)
            BasicMovement();
        else
        {
            rb.velocity = Vector3.zero; 
        }
    }


    public PlayerState GetState()
    {
        return this.plState;
    }

    public bool IsDead()
    {
        if(this.plState == PlayerState.DEAD)
        {
            return true;
        }
        return false;
    }

    public bool IsWinner()
    {
        if (this.plState == PlayerState.WINNER)
        {
            return true;
        }
        return false;
    }


    void BasicMovement()
    {
        if (direction.Equals(Vector3.zero))
        {
            Debug.Log("Velocity not set, the player won't move");
            animator.SetBool("isWalking", false);
            return;
        }
        animator.SetBool("isWalking", true);
        rb.velocity = direction.normalized * magnitude;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Finish"))
        {
            plState = PlayerState.WINNER;
            manager.FinishLevel();
        }

        if (other.CompareTag("Death"))
        {
            plState = PlayerState.DEAD;
            manager.FinishLevel();
        }

        animator.SetBool("isWalking", false);
        rb.velocity = Vector3.zero;
        
    }
}
