using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {

    [SerializeField] private float damage = 20f;
    [SerializeField] private float range = 10f;
    private Camera camera;

	// Use this for initialization
	void Start () {
        camera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSpell()
    {
        EnemyHealth enemy = null;
        Tree tree = null;
        GetAimingEntity(out enemy, out tree);
        if(enemy)
        {
            enemy.Damage(damage);
            //@TODO:particle related code: 
            /*
             *  fire around book 
             *  fire around player
             *  for only a small amount of time
             */
            Debug.Log("damaged an enemy with magic!");
        }
        else if(tree)
        {
            tree.ToggleFire();
            Debug.Log("Toggled tree's fire!");
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

        Ray r = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(r, out hitInfo, range, layerMask);

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
}
