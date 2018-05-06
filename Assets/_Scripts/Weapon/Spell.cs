using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
    
    public float damage = 20f;
    public static float sDamage;
    [SerializeField] private float range = 10f;
    public Transform throwOrigin;
    private Animator animator;
    private Camera camera;
    public static bool damaging = false;
    [HideInInspector] public EnemyHealth currentAimEnemy = null;
    [HideInInspector] public Tree currentAimTree = null;
    [HideInInspector] public Vector3 currentAimAt;
    public ParticleSpawner spawner;
    public static float charge = 100f;
    public float shotCost = 20f;

	// Use this for initialization
	void Start () {
        camera = GetComponentInChildren<Camera>();
        animator = GetComponent<Animator>();
        sDamage = damage;
	}

    public void OnSpell()
    {
        //only attack if not already attacking
        if (!animator.GetBool("swing") && charge > shotCost)
        {
            animator.SetBool("swing", true);
            animator.SetBool("isLongRange", true);
            currentAimEnemy = null;
            currentAimTree = null;
            GetAimingEntity(out currentAimEnemy, out currentAimTree);
        }
    }

    public void GetAimingEntity(out EnemyHealth _enemy, out Tree _tree)
    {
        //cast a ray in the direction of cursor and see if it collides with any enemies/trees
        //only cast rays against enemies/trees since its what we're interested (layer 11)
        //raycast itself takes care of "attack range" - ray will only be that long,
        // if enemies/trees are out of range, they won't be considered and the hit will be "null"

        int pMask = 1 << 11;
        int tMask = 1 << 12;

        int layerMask = pMask | tMask;
        int genLayerMask = 1 << 10;
        genLayerMask = ~genLayerMask;

        Ray r = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo, generalHit;
        Physics.Raycast(r, out hitInfo, range, layerMask);
        Physics.Raycast(r, out generalHit, Mathf.Infinity, genLayerMask);

        Vector3 direction = generalHit.point - spawner.originTransform.position;
        Debug.DrawRay(spawner.originTransform.position, direction, Color.blue, 3f);

        //save actual aiming at point as current aiming at
        currentAimAt = generalHit.point;

        _tree = null;
        _enemy = null;

        if (hitInfo.collider != null)
        {
            switch(hitInfo.collider.tag)
            {
                case "Enemy":
                    _enemy = hitInfo.collider.GetComponent<EnemyHealth>();
                    _tree = null;
                    break;
                case "Tree":
                    _tree = hitInfo.collider.GetComponent<Tree>();
                    _enemy = null;
                    break;
                default:
                    _tree = null;
                    _enemy = null;
                    break;
            }
        }
    }


    void OnMagicBegin()
    {
        damaging = false;
    }

    void OnMagicBeginHit()
    {
        Debug.Log("Aimed at: " + currentAimAt);
        spawner.SpawnParticleFromPool();
        damaging = true;
        charge -= shotCost;
        /*
         * hits are managed by the particles themselves now
        if(currentAimEnemy)
        {
            currentAimEnemy.Damage(damage);
            Debug.Log("damaged an enemy with magic!");
        }
        else if(currentAimTree)
        {
            currentAimTree.ToggleFire();
            Debug.Log("Toggled tree's fire!");
        }
        */
    }

    void OnMagicEndHit()
    {
        //if this is enabled, valid long range kills will not be counted
        //damaging = false;
    }
}
