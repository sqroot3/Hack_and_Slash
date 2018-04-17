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
        EnemyHealth enemy = getAimingEnemy();
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
    }

    public EnemyHealth getAimingEnemy()
    {
        //cast a ray in the direction of cursor and see if it collides with any enemies
        //only cast rays against enemies since its what we're interested (layer 11)
        //raycast itself takes care of "attack range" - ray will only be that long,
        // if enemies are out of range, they won't be considered and the hit will be "null"

        int layerMask = 1 << 11;

        Ray r = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(r, out hitInfo, range, layerMask);

        if(hitInfo.collider != null && hitInfo.collider.tag == "Enemy")
        {
            EnemyHealth enemy = hitInfo.collider.GetComponent<EnemyHealth>();
            return enemy;
        }

        return null;
    }
}
