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
        Debugger.DebugMessage("PlayerHealth", "Player has lost " + amount + " hp.Current hp: " + currentHealth);
        if (currentHealth <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        Debugger.DebugMessage("PlayerHealth", "Player has died");
        Manager.playerDied = true;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        Debugger.DebugMessage("PlayerHealth", "Player has recovered " + amount + " hp. Current hp: " + currentHealth);
    }
}
