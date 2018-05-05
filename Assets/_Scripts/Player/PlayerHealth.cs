using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    private float currentHealth;
    
    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        Manager.playerDied = true;
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
            if(enemy.isSwinging() && enemy.damaging)
            {
                Debug.Log("Enemy has damaged the player with melee damage");
                Damage(enemy.meleeTouchDamage);
            }
        }
    }
}
