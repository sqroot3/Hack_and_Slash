using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour {

    private Animator anim;
    private Rigidbody playerRB;
    private const string AttackParam = "isAttacking";
    private const string SwordState = "Sword";
    private bool isSwing = false;

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
        //if not already swinging
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName(SwordState))
        {
            anim.SetBool(AttackParam, true);
            isSwing = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //hit an enemy/sth - reset anim
        if(other.tag == "Enemy" && isSwing)
        {
            Debug.Log("Hit enemy!");
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            EnemyMovement movement = other.GetComponent<EnemyMovement>();
            //Sneak attack will instantly kill enemies
            float damage = (movement.IsPlayerBehind()) ? 9999f : 20f;
            enemy.Damage(damage);
        }
        if(other.tag == "Tree" && isSwing)
        {
            Debug.Log("Hit tree & toggled it's fire!");
            Tree tree = other.GetComponent<Tree>();
            tree.ToggleFire();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        isSwing = false;
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

    private void Update()
    {
        //Debug.Log("OnSwing: " + anim.GetBool(AttackParam));
    }
}
