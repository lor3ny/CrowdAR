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
    private PlayerState plState;
    private Vector3 direction;
    private LevelManager manager;

    public float magnitude;
    public Transform goalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        direction = goalPos.position - transform.position;
        plState = PlayerState.MOVING;

        magnitude += Random.Range(0.01f, 0.1f);
    }

    void Update()
    {
        if (plState == PlayerState.MOVING)
            BasicMovement();
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
            return;
        }
        rb.velocity = direction.normalized * magnitude;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            plState = PlayerState.WINNER;
            manager.FinishLevel();
        }

        Debug.Log("Touched");

        if (other.GetComponent("Death"))
        {
            plState = PlayerState.DEAD;
            manager.FinishLevel();
        }
    }
}
