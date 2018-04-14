using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    /*
     * For AI: whenever deciding what to go to,
     * evaluate distance to closest tree 
     * 
     */

    [SerializeField] private GameObject player;
    [SerializeField] private int treeRayColor;
    private PlayerMovement playerMovement;
    private EnemyAttack attack;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        attack = GetComponent<EnemyAttack>();
        playerMovement = player.GetComponent<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {

        //BOTH IF: enemy is facing player && the player is in "detection range"
        if (!IsPlayerBehind() && IsPlayerInRange())
        {
            //player is now detected, AI should now get closer to him
            agent.SetDestination(player.transform.position);
            if(attack.IsInAttackRadius())
            {
                //attack only if in attack radius
                attack.OnAttack();
            }
            
        }
        //IF no player in sight to go and there's at least one tree burning
        else if(Manager.numOfBurningTrees > 0)
        {
            //go to closest burning tree
            Tree t = getClosestBurningTree();
            agent.SetDestination(t.transform.position);
            attack.State = 0;

        }
        //ELSE, default to your calm/patrol state depending on enemy type
        else
        {
            //set it back to calm, and stay still
            //@TODO: implement a "patrolling" state - enemies of type dynamic should revert to it
            //static enemies will stand still
            agent.SetDestination(transform.position);
            attack.State = 0;
        }
        
        /*

        //Demo - draw ray to closest tree
        if(Input.GetKeyDown(KeyCode.T))
        {
            Tree t = getClosestTree();
            if(t)
            {
                Vector3 vecET = t.transform.position - transform.position;
                Debug.DrawRay(transform.position, vecET, getTreeRayColor(treeRayColor));
                agent.SetDestination(player.transform.position);
            }
        }
        //Demo - draw ray to closest burning tree
        if (Input.GetKeyDown(KeyCode.P))
        {
            Tree t = getClosestBurningTree();
            if (t)
            {
                Vector3 vecET = t.transform.position - transform.position;
                Debug.DrawRay(transform.position, vecET, getTreeRayColor(treeRayColor));
            }
        }
        */
    }

    public bool IsPlayerBehind()
    {
        //If DP b/w enemy's lookat and the vector from enemy to player is negative ( < -0.5f)
        //Then player is behind enemy
        Vector3 vecLookat = transform.forward;
        Vector3 vecEP = player.transform.position - transform.position;

        return (Vector3.Dot(vecEP, vecLookat) < -0.5f);
    }

    public bool IsPlayerInRange()
    {
        return (Mathf.Abs(Vector3.Distance(transform.position, playerMovement.transform.position)) <= attack.DetectRadius);
    }

    //determine closest tree
    public Tree getClosestTree()
    {
        float smallestDistance = 999999f;
        Tree closest = null;
        foreach(Tree t in Manager.trees)
        {
            if(t.distance(transform.position) < smallestDistance)
            {
                closest = t;
                smallestDistance = t.distance(transform.position);
            }
        }
        return closest;
    }

    //determine closest burning tree
    public Tree getClosestBurningTree()
    {
        float smallestDistance = 999999f;
        Tree closest = null;
        foreach (Tree t in Manager.trees)
        {
            if (t.distance(transform.position) < smallestDistance && t.OnFire)
            {
                closest = t;
                smallestDistance = t.distance(transform.position);
            }
        }
        return closest;
    }

    //demo - get color for closest tree ray
    public Color getTreeRayColor(int a)
    {
        switch(a)
        {
            case 0:     return Color.black;
            case 1:     return Color.white;
            case 2:     return Color.red;
            case 3:     return Color.green;
            default:    return Color.white;
        }
    }

}
