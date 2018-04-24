using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour {
    //strike time is time to count # hits for attack
    [SerializeField] private float strikeTime = 1f;
    [SerializeField] private float strikeDamage = 20f;
    private Animator anim;
    private Rigidbody playerRB;
    private const string AttackParam = "isAttacking";
    private const string SwordState = "Sword";
    private bool isSwing = false;
    private int swings = 0;

	void Awake()
    {
        anim = GetComponent<Animator>();
        if(playerRB)
        {
            Debug.Log("Player RB not assigned");
        }
        //sword = GetComponentInChildren<BoxCollider>();
    }

    /*
    public void OnSwing()
    {
        //if not already swinging
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName(SwordState))
        {
            anim.SetBool(AttackParam, true);
            isSwing = true;
        }
        //is midswing, check for second attack here
        else
        {
            Debug.Log("Second hit combo!");
        }
    }
    */

    public void OnSwing()
    {
        //this is called whenever player presses the attack button
        //should start the coroutine if not already on it
        //else, do nothing
        //if not already swinging
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(SwordState) && !isSwing)
        {
            StartCoroutine(SwordSwing());
            isSwing = true;
        }
        //are in the middle of swing, want to increase this
        else
            ++swings;
    }

    public IEnumerator SwordSwing()
    {
        swings = 1;
        yield return new WaitForSeconds(strikeTime);
        //At this point, handle animation states according to value of swings
        //@TODO: may want to use a swings variable in the animator
        // to handle what anim to go to
        Debug.Log("Swung " + swings + " times");
        isSwing = false;
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
            float damage = (movement.IsPlayerBehind()) ? 9999f : strikeDamage;
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
        Debug.Log("OnSwing: " + anim.GetBool(AttackParam));


    }
}
