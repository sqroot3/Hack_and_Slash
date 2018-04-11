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
    [SerializeField] private int treeRayColor;
    private PlayerMovement playerMovement;
    private EnemyAttack attack;

	// Use this for initialization
	void Start () {
        attack = GetComponent<EnemyAttack>();
        playerMovement = player.GetComponent<PlayerMovement>();
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

        //Demo - draw ray to closest tree
        if(Input.GetKeyDown(KeyCode.T))
        {
            Tree t = getClosestTree();
            if(t)
            {
                Vector3 vecET = t.transform.position - transform.position;
                Debug.DrawRay(transform.position, vecET, getTreeRayColor(treeRayColor));
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
    }

    private bool IsPlayerBehind()
    {
        Vector3 vecEP = player.transform.position - transform.position;

        return !(vecEP.z > 0.30f);
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
