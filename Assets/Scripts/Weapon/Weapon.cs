using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour {

    private Animator anim;
    private Rigidbody playerRB;
    private const string AttackParam = "isAttacking";

	void Awake()
    {
        anim = GetComponent<Animator>();
        if(playerRB)
        {
            Debug.Log("Player RB not assigned");
        }
        //sword = GetComponentInChildren<BoxCollider>();
    }

    public void OnSwing()
    {
        anim.SetBool(AttackParam, true);
        //anim.SetBool(AttackParam, false);
    }

    public void OnTriggerEnter(Collider other)
    {
        //hit an enemy/sth - reset anim
        Debug.Log(other.tag);
        anim.SetBool(AttackParam, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Enemy")
        {
            playerRB.velocity = Vector3.zero;
            collision.collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Touched an enemy!");
        }
    }

}
