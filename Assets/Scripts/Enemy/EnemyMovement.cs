using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] private GameObject player;
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
	}

    private bool IsPlayerBehind()
    {
        Vector3 vecEP = player.transform.position - transform.position;

        if (vecEP.z > 0.30) //behind player
            return true;

        return false;
    }
}
