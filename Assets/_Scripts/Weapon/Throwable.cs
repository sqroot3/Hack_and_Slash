using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {

    private float lifetime = 2f;
    public EnemyAttack owner;
    private Rigidbody rb;
    public Transform target;
    public float speed = 3f;
    private bool moving = false;
    private bool damaging = false;
    private Vector3 originalPosition;
    public float chargeTime;
    public float damage;
    public float spinSpeed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = new Vector3(0f, 0f, 0f);
    }

    private void WheelMovement()
    {
        // Gets vector from missile to target's position, and normalizes it (i.e to get a sense of the direction of path towards player) 
        Vector3 particleDirection = target.position - rb.position;
        particleDirection.Normalize();

        rb.velocity = particleDirection * speed;
        Debug.DrawRay(transform.position, particleDirection, Color.black);
    }

    void WheelSpin()
    {
        transform.Rotate(0, 0, spinSpeed, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Enemy hit player with a wheel!");
            other.GetComponent<Animator>().SetBool("damaged", true);
            other.GetComponent<PlayerHealth>().Damage(damage);
            Finish();
        }
    }

    private void FixedUpdate()
    {
        if(moving)
        {
            WheelMovement();
            WheelSpin();
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
        damaging = false;
        moving = false;
        transform.position = 1000* Random.insideUnitSphere;
        owner.resetLongRange();
        StartCoroutine(Charge());
    }

    private IEnumerator SelfDestroy(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Finish();
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(chargeTime);
        transform.localPosition = originalPosition;
        EnemyAttack.charged = true;
        owner.resetWheelVisibility();
    }
}
