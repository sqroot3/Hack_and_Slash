using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    private Rigidbody rb;

    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private bool isJoystick;
    [SerializeField] private bool airControl; //can steer mid-air
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject cameraRig;

    private CameraMovement camera;
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
            Debugger.DebugMessage("PlayerMovement", "Ground check transform not assigned!");

        camera = cameraRig.GetComponent<CameraMovement>();
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
        Debugger.DebugMessage("PlayerMovement", "Grounded: " + grounded + " Jump: " + jump);

        //forward/backward & sideways movement
        if(grounded || airControl)
        {
            //movement wrt camera's lookat
            rb.velocity = transform.forward * movement[0] * moveSpeed + transform.right * movement[1] * moveSpeed;
        }
        
        //vertical movement
        if(grounded && jump)
        {
            rb.AddForce(new Vector3(0f, jumpForce)); 
            grounded = false;
        }
    }

}
