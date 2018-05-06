using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    private float currentHealth;
    private Animator animator;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        animator.SetBool("alive", true);
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Hit! CH: " + currentHealth);
        if (currentHealth <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        Debug.Log("You died!");
        animator.SetBool("alive", false);

    }

    public void Heal(float amount)
    {
        currentHealth += amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy_Weapon")
        {
            EnemyAttack enemy = other.GetComponentInParent<EnemyAttack>();
            if (enemy.isSwinging() && enemy.damaging)
            {
                animator.SetBool("damaged", true);
                Debug.Log("Enemy has damaged the player with melee damage");
                Damage(enemy.meleeTouchDamage);

            }
        }
    }
   
    void OnHitBegin()
    {

    }

    void OnHitEnd()
    {
        animator.SetBool("damaged", false);
    }


    void OnDeathFinish()
    {
        Manager.playerDied = true;
    }

    void OnDeathStart()
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();
        movement.enabled = false;
    }
}
