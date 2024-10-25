using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int playersNumber = 0;
    private GameObject[] players;

    // PUBLIC FUNCTIONS

    public void StartLevel()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PlayerController plController = player.GetComponent<PlayerController>();
            plController.StartGame();
        }
    }

    // Every player calls this function when reaches the ending point, if some players are still playing it fails
    public void FinishLevel()
    {

        int deathsCount = DeadPlayers();
        int winnersCount = WinnerPlayers();
        int playersCount = playersNumber;

        if (winnersCount == playersCount)
        {
            // No player is dead, and the game is finished WON
            EndingRoutine(winnersCount, deathsCount, true);
            return;
        }

        if (deathsCount == playersCount)
        {
            // Every players is dead, the game is finished LOSE
            EndingRoutine(winnersCount, deathsCount, false);
            return;
        } 
        
        if(winnersCount + deathsCount == playersCount)
        {
            // Some deads and some winners, and the game is finished PARTIAL WIN
            if(deathsCount > playersCount / 2)
            {
                EndingRoutine(winnersCount, deathsCount, false);
            }
            else
            {
                EndingRoutine(winnersCount, deathsCount, true);
            }
        }
        Debug.Log("Finish Called! players: " + playersCount +" winners: " + winnersCount + " deaths: " + deathsCount);
        return;
    }


    // PRIVATE FUNCTIONS
    private void EndingRoutine(int winners, int deaths, bool haveWon)
    {
        if (haveWon)
        {
            Debug.Log("Winners: " + winners + " Deaths: " + deaths + " YOU WON!");
        } else
        {
            Debug.Log("Winners: " + winners + " Deaths: " + deaths + " YOU LOSE!");
        }
        //Ending UI and start next level
    }

    private int DeadPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int deaths = 0;
        foreach(GameObject player in players)
        {
            PlayerController plController = player.GetComponent<PlayerController>();

            if (plController.IsDead())
            {
                deaths++;
            }
        }
        return deaths;
    }

    private int WinnerPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int winners = 0;
        foreach (GameObject player in players)
        {
            PlayerController plController = player.GetComponent<PlayerController>();

            if (plController.IsWinner())
            {
                winners++;
            }
        }
        return winners;
    }
}
