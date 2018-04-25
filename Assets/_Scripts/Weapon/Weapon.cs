using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour {

    [SerializeField] private float strikeDamage;
    private Animator animator;
    private Rigidbody playerRB;
    private readonly int hashAttacking = Animator.StringToHash("attacking");

	void Awake()
    {
        animator = GetComponentInParent<Animator>();
        if(playerRB)
        {
            Debug.Log("Player RB not assigned");
        }
    }
    
    public void OnSwing()
    {
        animator.SetBool(hashAttacking, true);
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
        if (other.tag == "Enemy" && !isSwinging())
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();

            Vector3 hitLocation = new Vector3(other.transform.position.x, other.transform.position.y + enemy.labelHeight, other.transform.position.z);

            OnTargetHit(enemy, hitLocation);
            Debug.DrawLine(hitLocation, enemy.transform.forward);

        }
        else if (other.tag == "Tree" && !isSwinging())
        {
            Debug.Log("Hit tree & toggled it's fire!");
            Tree tree = other.GetComponent<Tree>();
            tree.ToggleFire();
        }
    }

    void OnTargetHit(EnemyHealth target, Vector3 location)
    {
        Debug.Log("Hit " + target + " with sword!");
        target.Damage(strikeDamage);
        target.hitContainer.active = true;
        target.hitContainer.transform.position = location;
        target.hitMesh.text = "+ " + strikeDamage;
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
        return !animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking");
    }
}
