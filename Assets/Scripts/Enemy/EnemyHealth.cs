using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        Debugger.DebugMessage("EnemyHealth", "Enemy has lost " + amount + " hp.Current hp: " + currentHealth);
        if (currentHealth <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        Debugger.DebugMessage("EnemyHealth", "Enemy has died");
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        Debugger.DebugMessage("EnemyHealth", "Enemy has recovered " + amount + " hp. Current hp: " + currentHealth);
    }
}
