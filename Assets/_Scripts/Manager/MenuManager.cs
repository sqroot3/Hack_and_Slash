using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public GameObject credits;

    public void OnDifficultyChange(int newLevel)
    {
        Manager.difficulty = newLevel;
    }

    public void OnGameLoad()
    {
        SceneManager.LoadScene("Convo");
    }

    public void OnCredits()
    {
        credits.SetActive(true);
    }

    public void OnGameQuit()
    {
        Application.Quit();
    }
}
