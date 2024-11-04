using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance; 

    private List<PlayerController> allZombies = new List<PlayerController>();

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method for each zombie to register itself with the manager
    public void RegisterZombie(PlayerController zombie)
    {
        if (!allZombies.Contains(zombie))
        {
            allZombies.Add(zombie);
        }
    }

    // Method to check if all zombies are in WINNER or DEAD states
    public bool AllZombiesInWinnerOrDeadState()
    {
        foreach (var zombie in allZombies)
        {
            if (zombie.plState != PlayerState.WINNER && zombie.plState != PlayerState.DEAD)
            {
                return false;  
            }
        }
        return true;  
    }
}
