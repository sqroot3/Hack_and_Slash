using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image[] difficulties;
    private bool skipDialog = true;

    public void OnDifficultyChange(int newLevel)
    {
        //need to change color too so that its obvious as to what difficulty is selected
        Manager.difficulty = newLevel;
        foreach (Image i in difficulties)
            i.color = Color.white;

        difficulties[newLevel].color = Color.red;


    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Credits")
            StartCoroutine(ReturnToMenu(3f));
    }

    public void OnDialogToggle()
    {
        skipDialog = !skipDialog;
    }

    public void OnGameLoad()
    {
        string sceneName = (skipDialog) ? "Manager" : "Convo";
        SceneManager.LoadScene(sceneName);
    }

    public void OnCredits()
    {
        //load a separate credit scene
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void OnGameQuit()
    {
        Application.Quit();
    }

    IEnumerator ReturnToMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
