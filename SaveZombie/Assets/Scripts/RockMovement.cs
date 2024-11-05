using UnityEngine;

public class RockMovement : MonoBehaviour
{
    public float speed = 0.00004f; // Speed of the rock
    private Vector3 targetPosition; // The position of the zombie when the rock was spawned
    private bool targetSet = false; // To check if a target is set
    private float timeAlive; // Timer for how long the rock has been alive
    public float totalLifetime = 20f; // Total time the rock should exist

    // Direction to keep the rock moving
    private Vector3 movementDirection;

    public void InitializeTarget()
    {
        // Find all zombies in the scene tagged as "Zombie"
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Player");

        if (zombies.Length > 0)
        {
            // Pick a random zombie from the list
            GameObject randomZombie = zombies[Random.Range(0, zombies.Length)];
            targetPosition = randomZombie.transform.position;
            targetSet = true; // Mark the target as set
            // Set the movement direction towards the zombie
            movementDirection = (targetPosition - transform.position).normalized;
        }
        else
        {
            Destroy(gameObject); // No zombies found, destroy the rock
        }
    }

    void Update()
    {
        // Only move if a target position has been set
        if (targetSet)
        {
            // Move the rock in the set direction
            transform.position += movementDirection * speed * 0.1f * Time.deltaTime;

            // Increase the alive time
            timeAlive += Time.deltaTime;

            // Destroy the rock after its total lifetime
            if (timeAlive >= totalLifetime)
            {
                Destroy(gameObject); // Destroy the rock after the total lifetime
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the rock collides with a zombie
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject); // Destroy the zombie
            Destroy(gameObject); // Destroy the rock
        }

        // Check if the rock collides with the wall
        if (other.CompareTag("Wall")) // Make sure to use the correct tag
        {
            Destroy(gameObject); // Destroy the rock
        }
    }
}
