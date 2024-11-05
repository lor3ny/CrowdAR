using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab;       // Assign your rock prefab in the Inspector
    public float spawnInterval = 30f;    // Time interval between rock spawns
    public Vector2 spawnAreaSize = new Vector2(10f, 5f); // Area for random spawning

    private float timer;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {

        if (AllPlayersInEndState())
        {
            return; // Stop spawning rocks
        }

        timer += Time.deltaTime;

        // Spawn a rock at each interval
        if (timer >= spawnInterval)
        {
            SpawnRock();
            timer = 0f;
        }
    }

    void SpawnRock()
    {
        // Random spawn position within defined area
        Vector3 spawnPos = new Vector3(
            transform.position.x + Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            transform.position.y,
            transform.position.z + Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        GameObject rockInstance = Instantiate(rockPrefab, spawnPos, Quaternion.identity);

        // Initialize the rock's movement towards a random zombie's position
        RockMovement rockMovement = rockInstance.GetComponent<RockMovement>();
        if (rockMovement != null)
        {
            rockMovement.InitializeTarget();
        }
    }

    private bool AllPlayersInEndState()
    {
        // Find all players in the scene tagged as "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Check each player's state
        foreach (GameObject player in players)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Check if the player is in MOVING state
                if (playerController.GetState() == PlayerController.PlayerState.MOVING || playerController.GetState() == PlayerController.PlayerState.IDLE)
                {
                    return false; // At least one player is still moving
                }
            }
        }

        return true; // All players are either DEAD or WINNER
    }
}
