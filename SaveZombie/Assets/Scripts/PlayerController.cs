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
    private Transform goalPos;

    public float magnitude;
    public int playerID;

    private Animator animator;

    private void Awake()
    {
        goalPos = GameObject.Find("t"+playerID).transform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        plState = PlayerState.IDLE;
        magnitude += Random.Range(0.0001f, 0.001f);
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

    public void StartGame()
    {
        if (!gameObject.activeSelf) return;

        plState = PlayerState.MOVING;
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
        direction = goalPos.position - transform.position;
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
            if (IsCollidingWithBridge())
            {
                // If colliding with the bridge, do not set the state to DEAD
                Debug.Log("Player is safe on the bridge!");
                return; // Exit the method to prevent death
            }
            plState = PlayerState.DEAD;
            manager.FinishLevel();
        }

        if (other.CompareTag("Rock"))
        {
            plState = PlayerState.DEAD;
            Debug.Log("Player died from the rocks");
            manager.FinishLevel();
        }


        animator.SetBool("isWalking", false);
        rb.velocity = Vector3.zero;
        
    }

    private bool IsCollidingWithBridge()
    {
        // Check if the player is colliding with an object tagged as "Bridge"
        // You may need to modify this based on your specific bridge tag
        return Physics.CheckSphere(transform.position, 0.5f, LayerMask.GetMask("modelObject"));
    }
}
