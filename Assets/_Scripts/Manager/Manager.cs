using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public static int difficulty;

    /*
     * Player "subclass" - needs to take care of player's attack/move/health 
     * 
     */
    public class Player
    {
        public PlayerMovement movement;
        public PlayerHealth health;
        public PlayerAttack attack;
        public CameraMovement camera;

        public Weapon sword;
        public Spell magic;
    }


    [SerializeField] private GameObject playerPrefab;
    public Player player = new Player();

    [SerializeField] private float gameTimer;

    /* GUI STUFF */
    public static Slider hpSlider;
    public Slider magicSlider;
    public Text timerText;
    public Text magicText;
    public Text swordText;


    public static List<Tree> trees = new List<Tree>();
    public List<EnemyAttack> enemies = new List<EnemyAttack>();
    public List<Throwable> wheels = new List<Throwable>();
    public static int numOfBurningTrees = 0;
    public static int aliveEnemies = 0;
    public static int magicKills = 0;
    public static int swordKills = 0;
    public static bool playerDied = false;
    private bool won = false;
    [SerializeField] private float startWait = 0f;
    [SerializeField] private float endWait = 0f;

    //Audio stuff
    public static AudioSource music_Source;
    public static AudioSource sfx_Source;

    //Music should be streaming, sfx should decompress on load
    public Text endMessage;

    private void Awake()
    {
        hpSlider = GameObject.FindGameObjectWithTag("HP_Slider").GetComponent<Slider>();
        music_Source = GameObject.FindGameObjectWithTag("Music_Source").GetComponent<AudioSource>();
        sfx_Source = GameObject.FindGameObjectWithTag("SFX_Source").GetComponent<AudioSource>();
        getEnemyReferences();
        getWheelReferences();
        adjustDifficulty();
        Debug.Log("Launched with difficulty: " + difficulty);
    }

    // Use this for initialization
    void Start () {

        //lock mouse into place, make it invisible
        //Note: should keep track of this on menu/settings etc
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
        GetPlayerData();
        playerDied = false;
        InitializeTrees();
        aliveEnemies = getNumberOfEnemies();
        StartCoroutine(GameLoop());
        magicSlider.value = Spell.charge;
	}

    void GetPlayerData()
    {
        player.movement = playerPrefab.GetComponent<PlayerMovement>();
        player.attack = playerPrefab.GetComponent<PlayerAttack>();
        player.health = playerPrefab.GetComponent<PlayerHealth>();
        player.sword = playerPrefab.GetComponentInChildren<Weapon>();
        player.magic = playerPrefab.GetComponent<Spell>();
        player.camera = playerPrefab.GetComponentInChildren<CameraMovement>();
    }

    void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        //reset appropriate values after reloading level
        swordKills = 0;
        magicKills = 0;
    }
	
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundStarting()
    {
        //init stuff here
        
        yield return new WaitForSeconds(startWait);
       
    }

    private IEnumerator RoundPlaying()
    {
        while (aliveEnemies > 0 && !playerDied && gameTimer > 0f)
        {
            //@TODO: implement game timer with UI (possibly slider) & remove comment below
            gameTimer -= Time.deltaTime;
            Spell.charge += Time.deltaTime * 2.0f;
            magicSlider.value = Spell.charge;
            magicText.text = string.Format("{0, 2}", magicKills);
            swordText.text = string.Format("{0, 2}", swordKills);

            timerText.text = TranslateTime(gameTimer);
            //Debug.Log("Time Left: " + gameTimer);
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        //decide if won/lost
        won = !(aliveEnemies > 0);
        string text = "";
        Color msgColor;
        if (won)
        {
            text = "YOU WIN!";
            msgColor = new Color(110, 171, 87, 255);
        }
        else
        {
            text = "YOU LOSE!";
            msgColor = new Color(221, 18, 18, 255);
        }

        endMessage.text = text;
        endMessage.color = msgColor;
        endMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(endWait);

        //load game
        playerDied = false;
        LoadLevel("Manager");
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

    string TranslateTime(float seconds)
    {
        string result = "";
        
        int minutes = (int)seconds / 60;
        int sec = (int)seconds % 60;

        result = string.Format("{0,2}:{1,2}", minutes.ToString("D2"), sec.ToString("D2"));

        return result;
    }

    void getEnemyReferences()
    {
        enemies.Clear();
        //find and populate trees
        GameObject[] result = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < result.Length; ++i)
        {
            EnemyAttack e = result[i].GetComponent<EnemyAttack>();
            enemies.Add(e);
        }
    }

    void getWheelReferences()
    {
        wheels.Clear();
        //find and populate trees
        GameObject[] result = GameObject.FindGameObjectsWithTag("Projectile");
        for (int i = 0; i < result.Length; ++i)
        {
            Throwable t = result[i].GetComponent<Throwable>();
            wheels.Add(t);
        }
    }

    void adjustDifficulty()
    {
        float[] multipliers = { 0.25f, 1f, 1.75f };
        foreach (EnemyAttack e in enemies)
            e.meleeTouchDamage *= multipliers[difficulty];
        foreach (Throwable t in wheels)
        {
            t.damage *= multipliers[difficulty];
            t.gameObject.SetActive(false);
            //t.GetComponent<GameObject>().SetActive(false);
        }
            
    }
}
