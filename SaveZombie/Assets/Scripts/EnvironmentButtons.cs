using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnvironmentButtons : MonoBehaviour
{
    
    private MarkersManager m_Managers;
    private LevelManager m_Levels;

    public GameObject buttonSet;
    public GameObject buttonReset;
    public Button buttonStart;

    private void Start()
    {
        m_Managers = GameObject.FindGameObjectWithTag("XROrigin").GetComponent<MarkersManager>();
        m_Levels = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        buttonSet.SetActive(true);
        buttonReset.SetActive(true);
    }

    public void SetEnvironment()
    {
        m_Managers.FixEnvSpawned();
        buttonSet.SetActive(false);
        buttonReset.SetActive(true);
    }

    public void ResetEnvironment()
    {
        m_Managers.ResetEnvSpawned();
        buttonSet.SetActive(true);
        buttonReset.SetActive(false);
    }

    public void StartEnvironment()
    {
        m_Levels.StartLevel();
        buttonStart.interactable = false;
    }

    private void Update()
    {
        if (m_Managers.envIsActive())
        {
            if (!buttonStart.interactable)
            {
                buttonStart.interactable = true;
            }
        } else
        {
            if (buttonStart.interactable)
            {
                buttonStart.interactable = false;
            }
        }
    }
}

