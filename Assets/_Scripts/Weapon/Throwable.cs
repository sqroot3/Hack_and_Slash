using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {

    private float lifetime = 3.5f;
    public EnemyAttack owner;
    private Rigidbody rb;
    public PlayerMovement target;
    public float speed = 3f;
    private bool moving = false;
    private bool damaging = false;
    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public Quaternion originalRotation;
    public float chargeTime;
    public float damage;
    public float spinSpeed = 5f;
    public Transform parent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = new Vector3(0f, 0f, 0f);
        originalRotation = transform.localRotation;
        parent = gameObject.GetComponentInParent<Transform>();
    }

    private void WheelMovement()
    {
        // Gets vector from missile to target's position, and normalizes it (i.e to get a sense of the direction of path towards player) 
        Vector3 particleDirection = target.torso.position - rb.position;
        particleDirection.Normalize();

        rb.velocity = particleDirection * speed;
        Debug.DrawRay(transform.position, particleDirection, Color.black);
    }

    void WheelSpin()
    {
        transform.Rotate(0, 0, spinSpeed, Space.Self);
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            WheelMovement();
            WheelSpin();
        }
    }

    public void BeginFlight()
    {
        //once in air on its own, it can damage & it can move
        //at this point the timer to autodestruct starts as well
        //no restrictions on flight
        rb.constraints = RigidbodyConstraints.None;
        damaging = true;
        moving = true;
        transform.rotation = Quaternion.identity;
        StartCoroutine(SelfDestroy(lifetime));
    }

    private IEnumerator SelfDestroy(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        //"destroying" is only hiding object, re-childing it, and moving it to init position
        //as well as adjusting damaging & moving conditions
        CleanUp();

    }

    void CleanUp()
    {
        //"destroying" is only hiding object, re-childing it, and moving it to init position
        //cant damage or move
        //recharge with time
        //restrictions back (y-movement)
        owner.StartCoroutine(owner.Charge(chargeTime));
        rb.constraints = RigidbodyConstraints.FreezeAll;
        damaging = false;
        moving = false;
        owner.DestroyProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Enemy hit player with a wheel!");
            other.GetComponent<Animator>().SetBool("damaged", true);
            other.GetComponent<PlayerHealth>().Damage(damage);
            CleanUp();
        }
        else if(other.tag == "Pivot")
        {
            other.GetComponent<Weapon>().owner.GetComponent<Animator>().SetBool("damaged", true);
            other.GetComponent<Weapon>().owner.GetComponent<PlayerHealth>().Damage(damage);
            CleanUp();
        }
        else if(other.tag != "Enemy" && other.tag != "Enemy_Weapon" && other.tag != "Projectile")
        {
            CleanUp();
        }
    }

        /*
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                Debug.Log("Enemy hit player with a wheel!");
                other.GetComponent<Animator>().SetBool("damaged", true);
                other.GetComponent<PlayerHealth>().Damage(damage);
                Finish();
            }
            //if it hits anything other than the player, just finish
            else if(other.tag != "Enemy" && other.tag != "Enemy_Weapon")
            {
                Debug.Log(other.tag);
                Finish();
            }
        }

        public void BeginFlight()
        {
            damaging = true;
            moving = true;
            StartCoroutine(SelfDestroy(lifetime));
        }

        public void Finish()
        {
            Debug.Log(owner.name + " Finish called");
            damaging = false;
            moving = false;
            transform.position = 1000* Random.insideUnitSphere;
            owner.resetLongRange();
            Debug.Log("Should be free of anim");
            StartCoroutine(Charge());
        }

        private IEnumerator SelfDestroy(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            Finish();
        }

        IEnumerator Charge()
        {
            Debug.Log(owner.name + " Charge called");
            yield return new WaitForSeconds(chargeTime);
            transform.localPosition = originalPosition;
            EnemyAttack.charged = true;
            owner.resetWheelVisibility();
        }
        */
    }
