using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    private float currentHealth;
    private Color deadColor = Color.black;
    private Color aliveColor = Color.white;
    private Material material;
    private Rigidbody rb;

    private void Awake()
    {
        currentHealth = startingHealth;
        material = GetComponent<MeshRenderer>().material;
        rb = GetComponent<Rigidbody>();
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        Debugger.DebugMessage("EnemyHealth", "Enemy has lost " + amount + " hp.Current hp: " + currentHealth);

        //Lerp its material color to show that it's been hit
        material.color = Color.Lerp(deadColor, aliveColor, currentHealth / startingHealth);
        
        if (currentHealth <= 0)
            OnDeath();
    }
    //hi
    public void OnDeath()
    {
        Debugger.DebugMessage("EnemyHealth", "Enemy has died");
        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        Debugger.DebugMessage("EnemyHealth", "Enemy has recovered " + amount + " hp. Current hp: " + currentHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
       //Debug.Log(collision.collider.tag);
    }
}
