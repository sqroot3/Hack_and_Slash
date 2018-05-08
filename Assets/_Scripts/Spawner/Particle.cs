using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    private Vector3 targetTransform;
    public Vector3 TargetTransform
    {
        get { return targetTransform; }
        set { targetTransform = value; }
    }

    private Rigidbody particleRB;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float spinSpeed = 5f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float selfDestructTimer = 2f;
    [SerializeField] private GameObject playerPrefab;
    public static bool damaging = false;

    private void Awake()
    {
        particleRB = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(SelfDestruct());
    }

    private void FixedUpdate()
    {
        ParticleMovement();
    }

    private void ParticleMovement()
    {
        // Gets vector from missile to target's position, and normalizes it (i.e to get a sense of the direction of path towards player) 
        Vector3 particleDirection = targetTransform - particleRB.position;
        particleDirection.Normalize();
        
        particleRB.velocity = particleDirection * speed;
        Debug.DrawRay(transform.position, particleDirection, Color.black);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTimer);
        ParticlePool.Instance.AddToPool(gameObject);
        //explode is a destruction function
        //Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        //hit an enemy anywhere
        if (other.tag == "Enemy" || other.tag == "Enemy_Weapon" || other.tag == "Wheel_Back")
        {
            Manager.sfx_Source.Stop();
            if (Spell.damaging)
            {
                EnemyHealth enemy = null;
                if (other.tag == "Enemy_Weapon" || other.tag == "Wheel_Back")
                    enemy = other.GetComponent<Enemy_Weapon>().health;
                else
                    enemy = other.GetComponent<EnemyHealth>();

                Vector3 hitLocation = new Vector3(other.transform.position.x, other.transform.position.y + enemy.labelHeight, other.transform.position.z);
                OnTargetHit(enemy, hitLocation);
            }
            ParticlePool.Instance.AddToPool(gameObject);
        }
        //hit a tree
        else if(other.tag == "Tree")
        {
            Manager.sfx_Source.Stop();
            if (Spell.damaging)
            {
                other.GetComponent<Tree>().ToggleFire();
            }
            ParticlePool.Instance.AddToPool(gameObject);
        }
        //hit a projectile (as in an enemy's wheel)
        else if(other.tag == "Projectile")
        {
            Manager.sfx_Source.Stop();
            ParticlePool.Instance.AddToPool(gameObject);
            //reset animation bla bla
            other.GetComponent<Throwable>().Finish();
        }
    }

    void OnTargetHit(EnemyHealth target, Vector3 location)
    {
        Debug.Log("Hit " + target + " with magic!");
        EnemyMovement movement = target.GetComponent<EnemyMovement>();
        float damage = Spell.sDamage;
        movement.animator.SetBool("damaged", true);
        target.Damage(damage, 1);
        target.hitContainer.active = true;
        target.hitContainer.transform.position = location;
        //calculate correct rotation so that the player can see the label right
        float rotation = (movement.IsPlayerBehind()) ? 0f : 180f;
        target.hitContainer.transform.localRotation = Quaternion.identity;
        target.hitContainer.transform.Rotate(new Vector3(0f, rotation, 0f));

        target.hitMesh.text = "+ " + damage;
        ParticleSpawner parent = GetComponentInParent<ParticleSpawner>();
        parent.DeleteMsg(target);
    }
    
}
