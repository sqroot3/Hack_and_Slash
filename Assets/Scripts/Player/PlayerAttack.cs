using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private Texture2D crosshair;
    [SerializeField] private float damage = 3.0f;
    [SerializeField] private GameObject book;
    
    private string aimAxis = "Aim";
    private string attackAxis = "Attack";
    
    private Weapon sword;
    private Spell magic;
    private int currentAttack = 0;
    //0 - sword
    //1 - magic


	// Use this for initialization
	void Start () {
        sword = GetComponentInChildren<Weapon>();
        magic = GetComponent<Spell>();
	}
	
	// Update is called once per frame
	void Update () {
        //If aiming, set current weapon to magic, if not, then sword
        if (Input.GetButton(aimAxis))
        {
            //@TODO: Need to fix texture to be accurate as mouse pointer is
            //Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);
            //free mouse when aiming
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currentAttack = 1;
            book.SetActive(true);
        }
        else
        {
            //re-center & "lock" mouse when not in "aim" mode
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currentAttack = 0;
            book.SetActive(false);
        }

        if(Input.GetButtonDown(attackAxis))
        {
            switch(currentAttack)
            {
                case 0:
                    sword.OnSwing();
                    break;
                case 1:
                    magic.OnSpell();
                    break;
            }
        }
            
	}
}
