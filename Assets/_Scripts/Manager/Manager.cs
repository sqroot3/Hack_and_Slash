using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
    
    
    [SerializeField] private float gameTimer;


    public static List<Tree> trees = new List<Tree>();
    public static int numOfBurningTrees = 0;
    public static int aliveEnemies = 0;
    public static bool playerDied = false;
    private bool won = false;
    [SerializeField] private float startWait = 0f;
    [SerializeField] private float endWait = 0f;


	// Use this for initialization
	void Start () {

        //lock mouse into place, make it invisible
        //Note: should keep track of this on menu/settings etc
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
        playerDied = false;
        InitializeTrees();
        aliveEnemies = getNumberOfEnemies();
        StartCoroutine(GameLoop());
	}

    void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
	
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        //win/lose scenario

        //@TODO: make win/loss implemented with gui too
        if (won)
        {
            Debug.Log("You won! Restarting level!");
        }
        else
        {
            Debug.Log("You lost! Restarting leveL!");
        }
        
        playerDied = false;
        LoadLevel("Manager");
    }

    private IEnumerator RoundStarting()
    {
        //init stuff here
        yield return startWait;
       
    }

    private IEnumerator RoundPlaying()
    {
        while (aliveEnemies > 0 && !playerDied && gameTimer > 0f)
        {
            //@TODO: implement game timer with UI (possibly slider) & remove comment below
            //gameTimer -= Time.deltaTime;
            Debug.Log("Time Left: " + gameTimer);
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        //decide if won/lost
        won = !(aliveEnemies > 0);
        yield return endWait;
    }
    
    void InitializeTrees()
    {
        //empty current tree array, reset # burning trees
        trees.Clear();
        numOfBurningTrees = 0;
        
        //find and populate trees
        GameObject[] result = GameObject.FindGameObjectsWithTag("Tree");
        for(int i = 0; i < result.Length; ++i)
        {
            Tree t = result[i].GetComponent<Tree>();
            trees.Add(t);
        }
    }

    int getNumberOfEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
