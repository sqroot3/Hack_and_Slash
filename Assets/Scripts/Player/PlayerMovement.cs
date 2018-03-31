using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //rigidbody instance - will move this
    private Rigidbody rb;

    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private bool isJoystick;
    [SerializeField] private bool airControl; //can steer mid-air
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float moveSpeed;

    private const float GroundedRadius = .2f;

    private string[] jumpAxis = { "Jump", "JumpJoy" };
    private bool jump; //input from jump
    private bool grounded;

    private string[] moveAxis = { "ForwardBack", "Sides"};
    private float[] movement = { 0f, 0f };
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (!groundCheck)
            Debug.LogError("Ground check transform not assigned!");
    }

    private void FixedUpdate()
    {
        grounded = false;

        if (groundCheck)
        {
            //Player is grounded if sphere overlap hits anything labelled "ground"
            Collider[] colliders = Physics.OverlapSphere(groundCheck.position, GroundedRadius, groundLayers);
            foreach(Collider c in colliders)
            {
                if (c.gameObject != gameObject)
                    grounded = true;
            }
        }
    }

    private void Update()
    {
        //get input from keyboard
        if (!isJoystick)
        {
            movement[0] = Input.GetAxis(moveAxis[0]);
            movement[1] = Input.GetAxis(moveAxis[1]);
            jump = Input.GetButtonDown(jumpAxis[0]);
        }

        Move(jump);
        jump = false;
    }

    public void Move(bool jump)
    {
        Debug.Log("Grounded: " + grounded + " Jump: " + jump);

        //forward/backward & sideways movement
        if(grounded || airControl)
        {
            rb.velocity = new Vector3(movement[1] * moveSpeed, rb.velocity.y, movement[0] * moveSpeed);
        }
        
        //vertical movement
        if(grounded && jump)
        {
            rb.AddForce(new Vector3(0f, jumpForce));
            grounded = false;
        }
    }

}
