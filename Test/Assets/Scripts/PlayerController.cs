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

    public float magnitude;
    public Transform goalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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


    void BasicMovement()
    {
        if (direction.Equals(Vector3.zero))
        {
            Debug.Log("Velocity not set, the player won't move");
            return;
        }
        rb.velocity = direction.normalized * magnitude;
    }
}
