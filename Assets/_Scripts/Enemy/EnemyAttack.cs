using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private float detectRadius = 2.0f;
    [SerializeField] private float attackRadius = 1.0f;
    private PlayerMovement playerMovement;
    private MeshRenderer renderer;
    private int state = 0;

    public int State
    {
        get { return state; }
        set { state = value; }
    }

    public float DetectRadius
    {
        get { return detectRadius; }
        set { detectRadius = value; }
    }
    //0 - calm (white)
    //1 - detected (yellow)
    //2 - attacking (red)
    //3 - patrolling (blue)
    //4 - tree (green)
    
    

    // Use this for initialization
    void Start () {
        playerMovement = player.GetComponent<PlayerMovement>();
        renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        /*
        switch (state)
        {
            case 0:
                renderer.material.color = Color.white;
                break;
            case 1:
                renderer.material.color = Color.yellow;
                break;
            case 2:
                renderer.material.color = Color.red;
                break;
            case 3:
                renderer.material.color = Color.blue;
                break;
            case 4:
                renderer.material.color = Color.green;
                break;
            default:
                renderer.material.color = Color.white;
                break;
        }
        */
    }

    public void OnAttack()
    {
        if(IsInAttackRadius())
        {
            Debug.Log("attacked!");
            //set its color to an "attacking" state - just for demo purposes
            State = 2;
        }
        else
        {
            //if out of reach, set its color back to white - demo purposes
            State = 0;
        }
    }

    public bool IsInAttackRadius()
    {
        //Get the distance from enemy to player
        float distance = Vector3.Distance(transform.position, playerMovement.transform.position);

        return (distance <= attackRadius);
    }
}
