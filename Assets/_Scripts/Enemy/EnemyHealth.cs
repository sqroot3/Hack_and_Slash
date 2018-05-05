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

    private void Awake()
    {
        currentHealth = startingHealth;
        rb = GetComponent<Rigidbody>();
        hitMesh = hitContainer.GetComponent<TextMesh>();
        hitMesh.color = Color.yellow;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        
        //apply damage anim to enemy here
        
        if (currentHealth <= 0)
            OnDeath();
    }
    
    public void OnDeath()
    {
        //apply death anim to enemy here

        Destroy(gameObject);
        Manager.aliveEnemies--;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
    }
}
