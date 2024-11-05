using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetCurrentLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TMP_Text mytext = GetComponent<TMP_Text>();
        mytext.SetText(SceneManager.GetActiveScene().buildIndex.ToString());
    }
}
