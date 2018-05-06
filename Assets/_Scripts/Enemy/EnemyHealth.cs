using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    public float labelHeight;
    public GameObject hitContainer;
    [HideInInspector] public TextMesh hitMesh;

    public float currentHealth;
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        currentHealth = startingHealth;
        rb = GetComponent<Rigidbody>();
        hitMesh = hitContainer.GetComponent<TextMesh>();
        hitMesh.color = Color.yellow;
        animator = GetComponent<Animator>();
        animator.SetBool("alive", true);
    }

    //0 - sword, 1 - magic
    public void Damage(float amount, int type)
    {
        currentHealth -= amount;

        Debug.Log("I was hit!" + currentHealth);

        if (currentHealth <= 0)
            OnDeath(type);
    }
    
    public void OnDeath(int type)
    {
        switch(type)
        {
            case 0:
                Manager.swordKills++;
                break;
            case 1:
                Manager.magicKills++;
                break;
            default: break;
        }
        //apply death anim to enemy here
        animator.SetBool("alive", false);
        animator.SetBool("damaged", true);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
    }

    void OnHitBegin()
    {
        animator.SetBool("damaged", true);
    }

    void OnHitEnd()
    {
        animator.SetBool("damaged", false);
    }

    void OnDeathFinish()
    {
        Destroy(gameObject);
        Manager.aliveEnemies--;
    }

    void OnDeathStart()
    {
        EnemyMovement movement = GetComponent<EnemyMovement>();
        movement.enabled = false;
    }
}
