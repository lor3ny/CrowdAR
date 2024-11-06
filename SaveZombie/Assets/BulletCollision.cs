using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the rock collides with a zombie
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the rock
        }

        // Check if the rock collides with the wall
        if (other.CompareTag("Wall")) // Make sure to use the correct tag
        {
            Destroy(gameObject); // Destroy the rock
        }
    }
}
