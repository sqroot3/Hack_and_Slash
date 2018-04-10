using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    /*
     * For AI: whenever deciding what to go to,
     * evaluate distance to closest tree 
     * 
     */

    [SerializeField] private GameObject player;
    private PlayerMovement playerMovement;
    private EnemyAttack attack;

    private List<Tree> trees;

	// Use this for initialization
	void Start () {
        attack = GetComponent<EnemyAttack>();
        playerMovement = player.GetComponent<PlayerMovement>();
        initializeTrees();
    }

    // Update is called once per frame
    void Update () {
        if (!IsPlayerBehind())
        {
            attack.OnAttack();
        }
        else
        {
            attack.setState(0);
        }
	}

    private bool IsPlayerBehind()
    {
        Vector3 vecEP = player.transform.position - transform.position;

        return !(vecEP.z > 0.30f);
    }

    void initializeTrees()
    {
        
    }

    //determine closest tree
    public Tree getClosestTree()
    {

        
    }
}
