using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int playersNumber = 0;
    private GameObject[] players;
    public AudioClip[] sounds;

    // PUBLIC FUNCTIONS

    private GameObject winUI;
    private GameObject loseUI;
    private GameObject gameUI;
    private TMP_Text points;
    private TMP_Text pointsShadow;
    private TMP_Text maxPointsShadow;
    private TMP_Text maxPoints;
    private AudioSource audiosource;

    private void Awake()
    {
        winUI = GameObject.Find("WinUI");
        loseUI = GameObject.Find("LoseUI");
        gameUI = GameObject.Find("GameUI");
        points = GameObject.Find("Points").GetComponent<TMP_Text>();
        maxPoints = GameObject.Find("MaxPoints").GetComponent<TMP_Text>();
        maxPointsShadow = GameObject.Find("MaxPoints_shadow").GetComponent<TMP_Text>();
        pointsShadow = GameObject.Find("Points_shadow").GetComponent<TMP_Text>();
        winUI.SetActive(false);
        loseUI.SetActive(false);
        gameUI.SetActive(true);
        maxPoints.SetText('/' + playersNumber.ToString());
        maxPointsShadow.SetText('/' + playersNumber.ToString());
    }

    public void StartLevel()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PlayerController plController = player.GetComponent<PlayerController>();
            plController.StartGame();
        }

        GameObject[] shooters = GameObject.FindGameObjectsWithTag("Shooter");
        foreach(GameObject shooter in shooters)
        {
            shooter.GetComponent<ShooterManager>().Begin();
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

        points.SetText(deathsCount.ToString());
        pointsShadow.SetText(deathsCount.ToString());
        return;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    // PRIVATE FUNCTIONS
    private void EndingRoutine(int winners, int deaths, bool haveWon)
    {
        audiosource = GameObject.Find("BackgroundMusicSource").GetComponent<AudioSource>();
        if (haveWon)
        {
            Debug.Log("Winners: " + winners + " Deaths: " + deaths + " YOU WON!");

            // Activate canvas with winners and deaths
            // give the change to retry or go on.
            gameUI.SetActive(false);
            winUI.SetActive(true);
            //GameObject.Find("WinnersCountInt").GetComponent<TMP_Text>().text = winners.ToString();
            GameObject.Find("DeathsCountInt").GetComponent<TMP_Text>().text = deaths.ToString();
            audiosource.Stop();
            audiosource.PlayOneShot(sounds[0]);

        } else
        {
            Debug.Log("Winners: " + winners + " Deaths: " + deaths + " YOU LOSE!");

            // Activate loosing canvas
            // give the change to retry or go on.
            gameUI.SetActive(false);
            loseUI.SetActive(true);
            //GameObject.Find("WinnersCountInt").GetComponent<TMP_Text>().text = winners.ToString();
            GameObject.Find("DeathsCountInt").GetComponent<TMP_Text>().text = deaths.ToString();
            audiosource.Stop();
            audiosource.PlayOneShot(sounds[1]);
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
