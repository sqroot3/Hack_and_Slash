using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private Texture2D crosshair;
    [SerializeField] private GameObject book;
    public AudioClip swordClip;
    

    private string aimAxis = "Aim";
    private string attackAxis = "Attack";
    private readonly int hashAttacking = Animator.StringToHash("attacking");

    private Weapon sword;
    private Spell magic;

    [SerializeField] private float attackTime = 1f;

    private int currentAttack = 0;
    //0 - sword
    //1 - magic

	
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
            if(Manager.playing)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            currentAttack = 0;
            book.SetActive(false);
        }

        if (Input.GetButtonDown(attackAxis))
        {
            switch(currentAttack)
            {
                case 0:
                    //@TODO: should combos be considered in tandem with an already going attack?
                    // i.e, before first swing ends, if player clicks attack, second swing, and so on
                    //StartCoroutine(sword.OnSwing(swings++));
                    sword.OnSwing();
                    break;
                case 1:
                    magic.OnSpell();
                    break;
            }
        }

    }

    void OnSlashBegin()
    {
        sword.damaging = false;
       
    }

    void OnSlashBeginDamage()
    {
        sword.damaging = true;
        Manager.sfx_Source.clip = swordClip;
        Manager.sfx_Source.Play();
    }

    void OnSlashEndDamage()
    {
        sword.damaging = false;
    }
}
