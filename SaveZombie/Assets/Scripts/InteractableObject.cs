using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class InteractableObject : MonoBehaviour
{


    [SerializeField]
    public string[] Obstacles;
    [SerializeField]
    public bool Obstacle;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT!");
        Debug.Log(other.gameObject.name);
        if (Obstacle) return;

        foreach (string obstacle in Obstacles)
        {

            if (other.name == obstacle)
            {
                // Do Some Actions, for now delete the obstacle
                other.gameObject.SetActive(false);
            }
        }
    }
    
}
