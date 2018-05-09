using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private float detectRadius = 2.0f;
    [SerializeField] private float attackRadius = 1.0f;
    public float meleeTouchDamage = 1f;
    private PlayerMovement playerMovement;
    private Animator animator;
    private int state = 0;
    [HideInInspector] public bool damaging = false;
    public GameObject backWheel;
    public GameObject armWheel;
    public bool charged = true;
    public AudioClip swingClip;


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
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Current state: " + state);
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
            if(!isSwinging())
            {
                animator.SetBool("isLongRange", false);
                animator.SetBool("swing", true);
            }
            //set its color to an "attacking" state - just for demo purposes
            State = 2;
        }
        else if(State == 1 && charged)
        {
            //long range attack if detected and if weapon is charged
            animator.SetBool("isLongRange", true);
            animator.SetBool("swing", true);
        }
        else
        {
            //if out of reach, set its color back to white - demo purposes
            State = 0;
        }
    }

    public bool IsInDetectRadius()
    {
        //Get the distance from enemy to player
        float distance = Vector3.Distance(transform.position, playerMovement.transform.position);

        return (distance <= detectRadius);
    }

    public bool IsInAttackRadius()
    {
        //Get the distance from enemy to player
        float distance = Vector3.Distance(transform.position, playerMovement.transform.position);

        return (distance <= attackRadius);
    }

    public bool isSwinging()
    {
        //is swinging if on attacking state on the attack layer (id 1)
        return animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Slash");
    }
    
    void OnSwingStart()
    {
        damaging = false;
    }

    void OnSwingBeginDamage()
    {
        damaging = true;
        Manager.sfx_Source.clip = swingClip;
        Manager.sfx_Source.Play();
    }

    void OnSwingEndDamage()
    {
        damaging = false;
        animator.SetBool("swing", false);
    }

    /*
    void OnThrowBegin()
    {
        //hide back wheel, turn on arm wheel
        backWheel.SetActive(false);
        armWheel.SetActive(true);
        charged = false;
    }

    void OnThrowLaunch()
    {
        //arm wheel goes flying at player :)
        Throwable _projectile = armWheel.GetComponent<Throwable>();
        _projectile.target = player.transform;
        _projectile.BeginFlight();
    }

    void OnThrowEnd()
    {
        animator.SetBool("swing", false);
    }

    public void resetLongRange()
    {
        animator.SetBool("swing", false);
        animator.SetBool("isLongRange", false);
    }

    public void resetWheelVisibility()
    {
        animator.SetBool("swing", false);
        backWheel.SetActive(true);
        //this is the problem for the shoulder bug
        armWheel.SetActive(false);
    }
    */

    void OnThrowBegin()
    {
        //hide back, show arm wheel
        //charge is now off
        backWheel.SetActive(false);
        armWheel.SetActive(true);
        charged = false;
    }

    void OnThrowLaunch()
    {
        //wheel is on its own now, is no longer child of hand
        
        Throwable _projectile = armWheel.GetComponent<Throwable>();
        _projectile.parent = armWheel.transform.parent;
        armWheel.transform.parent = null;
        _projectile.target = playerMovement;
        Debug.Log(playerMovement.transform.position);
        armWheel.SetActive(true);
        _projectile.BeginFlight();

    }

    public void DestroyProjectile()
    {
        Throwable _projectile = armWheel.GetComponent<Throwable>();
        armWheel.transform.parent = _projectile.parent;
        armWheel.SetActive(false);
        armWheel.transform.localPosition = _projectile.originalPosition;
        armWheel.transform.localRotation = _projectile.originalRotation;
    }

    public IEnumerator Charge(float time)
    {
        yield return new WaitForSeconds(time);
        //once charged, the back wheel "reappears"
        backWheel.SetActive(true);
        charged = true;
    }

    void LeaveThrowAnimation()
    {
        animator.SetBool("swing", false);
        animator.SetBool("isLongRange", false);
    }

    void OnThrowEnd()
    {
        //once done throwing, get out of animation
        LeaveThrowAnimation();
    }
}
