using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField] private float startingHealth = 100f;

    private float currentHealth;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        Debugger.DebugMessage("PlayerHealth", "Player has recovered " + amount + " hp. Current hp: " + currentHealth);
    }
}
