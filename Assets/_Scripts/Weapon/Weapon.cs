using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{

    public float strikeDamage = 10f;
    private Animator animator;
    private Rigidbody playerRB;
    private readonly int hashSwing = Animator.StringToHash("swing");
    [SerializeField] private float ignoreTime = 5f;
    private float currentTime;
    public bool damaging = false;
    public GameObject owner;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        if (playerRB)
        {
            Debug.Log("Player RB not assigned");
        }
        currentTime = ignoreTime;
    }

    public void OnSwing()
    {
        //only attack if not already attacking
        if(!animator.GetBool(hashSwing))
        {
            animator.SetBool(hashSwing, true);
            animator.SetBool("isLongRange", false);
        }
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Enemy")
        {
            playerRB.velocity = Vector3.zero;
            collision.collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Touched an enemy!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ( (other.tag == "Enemy" || other.tag == "Enemy_Weapon" ) && isSwinging())
        {
            if (damaging && currentTime < 0)
            {
                EnemyHealth enemy = null;
                if (other.tag == "Enemy_Weapon")
                {
                    enemy = other.GetComponent<Enemy_Weapon>().health;
                }
                else
                {
                    enemy = other.GetComponent<EnemyHealth>();
                }

                Vector3 hitLocation = new Vector3(other.transform.position.x, other.transform.position.y + enemy.labelHeight, other.transform.position.z);

                OnTargetHit(enemy, hitLocation);
                currentTime = ignoreTime;
                Debug.DrawLine(hitLocation, enemy.transform.forward);
            }
        }
        else if (other.tag == "Tree" && isSwinging())
        {
            if(damaging && currentTime < 0)
            {
                Debug.Log("Hit tree & toggled it's fire!");
                Tree tree = other.GetComponent<Tree>();
                tree.ToggleFire();
                currentTime = ignoreTime;
            }
        }
    }

    void OnTargetHit(EnemyHealth target, Vector3 location)
    {
        Debug.Log("Hit " + target + " with sword!");
        EnemyMovement movement = target.GetComponent<EnemyMovement>();
        float damage = (movement.IsPlayerBehind()) ? 999f: strikeDamage;
        target.Damage(damage, 0);
        target.hitContainer.active = true;
        target.hitContainer.transform.position = location;
        //calculate correct rotation so that the player can see the label right
        float rotation = (movement.IsPlayerBehind()) ? 0f : 180f;
        target.hitContainer.transform.localRotation = Quaternion.identity;
        target.hitContainer.transform.Rotate(new Vector3(0f, rotation, 0f));

        target.hitMesh.text = "+ " + damage;
        StartCoroutine(HideHitMessage(target));
    }

    private IEnumerator HideHitMessage(EnemyHealth target)
    {
        yield return new WaitForSeconds(1.5f);
        if (target)
            target.hitContainer.active = false;
    }

    bool isSwinging()
    {
        //is swinging if on attacking state on the attack layer (id 1)
        return animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Slash");
    }


    private void FixedUpdate()
    {
        currentTime -= Time.deltaTime;
    }
}
