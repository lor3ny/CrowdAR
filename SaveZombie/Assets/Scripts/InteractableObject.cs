using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class InteractableObject : MonoBehaviour
{


    [SerializeField]
    public string[] Obstacles;
    [SerializeField]
    public bool isBridge;

    private GameObject fixedBridge;


    public void SetFixedBridge(GameObject bridge)
    {
        this.fixedBridge = bridge;
    }

    private void OnTriggerEnter(Collider other)
    {

        foreach (string obstacle in Obstacles)
        {
            if (other.name == obstacle)
            {
                if (isBridge)
                {
                    fixedBridge.SetActive(true);
                } else
                {
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
    
}
