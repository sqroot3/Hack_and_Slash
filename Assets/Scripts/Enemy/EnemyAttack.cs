using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private float attackRadius = 2.0f;
    private PlayerMovement playerMovement;
    private MeshRenderer renderer;
    private int state = 0;
    //0 - calm
    //1 - attacking
    
    

    // Use this for initialization
    void Start () {
        playerMovement = player.GetComponent<PlayerMovement>();
        renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case 0:
                renderer.material.color = Color.white;
                break;
            case 1:
                renderer.material.color = Color.red;
                break;
            default:
                renderer.material.color = Color.white;
                break;
        }
    }

    public void OnAttack()
    {

        //Get the distance from enemy to player
        float distance = Vector3.Distance(transform.position, playerMovement.transform.position);

        //Debug.Log("Distance: " + distance);
        //if the distance is small enough to be covered by the attack radius
        if(distance <= attackRadius)
        {
            Debug.Log("attacked!");
            //set its color to an "attacking" state - just for demo purposes
            setState(1);
        }
        else
        {
            //if out of reach, set its color back to white - demo purposes
            setState(0);
        }
    }

    public void setState(int _state)
    {
        state = _state;
    }
}
