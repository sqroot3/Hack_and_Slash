using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private KeyCode key = KeyCode.Mouse1; //light attack
    [SerializeField]
    private float damage = 3.0f;
    [SerializeField]
    private Weapon currentWeapon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(key))
            currentWeapon.OnSwing();
	}
}
