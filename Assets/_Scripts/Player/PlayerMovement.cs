using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private float dirtMultiplier;
    [SerializeField] private float snowMultiplier;

    private float baseSpeed;
    private readonly int hashSpeed = Animator.StringToHash("speed");
    //private readonly int hashSprint = Animator.StringToHash("sprinting");

    private string sprintAxis = "Sprint";

    private void Awake()
    {
        baseSpeed = animator.GetFloat(hashSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //handle entering in snow/dirt area triggers
        if (other.tag == "Snow_Ground")
        {
            //player will be slower on snow
            animator.SetFloat(hashSpeed, animator.GetFloat(hashSpeed) - (animator.GetFloat(hashSpeed) * snowMultiplier)); 
        }
        else if (other.tag == "Dirt_Ground")
        {
            //player will be faster on dirt
            animator.SetFloat(hashSpeed, animator.GetFloat(hashSpeed) + (animator.GetFloat(hashSpeed) * dirtMultiplier));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // as soon as area is left, reset speed to normal speed
        if (other.tag == "Snow_Ground" || other.tag == "Dirt_Ground")
        {
            //Debug.Log("Called!");
            animator.SetFloat(hashSpeed, baseSpeed);
        }
    }

    private void Update()
    {
        /*
        if(Input.GetButton(sprintAxis))
        {
            animator.SetBool(hashSprint, true);
        }
        else
        {
            animator.SetBool(hashSprint, false);
        }
        */
    }

    /*
    private Rigidbody rb;


    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private bool isJoystick;
    [SerializeField] private bool airControl; //can steer mid-air
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dirtMultiplier;
    [SerializeField] private float snowMultiplier;
    [SerializeField] private GameObject cameraRig;
    [SerializeField] private float deathY = 0.45f;

    private float speed;

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
            Debug.Log("Ground check transform not assigned!");

        camera = cameraRig.GetComponent<CameraMovement>();
        speed = moveSpeed;
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
        
        if(transform.position.y <= deathY && !Manager.playerDied)
        {
            PlayerHealth health = GetComponent<PlayerHealth>();
            health.OnDeath();
            Debug.Log("Died from a fall");
        }

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
        //Debug.Log("Grounded: " + grounded + " Jump: " + jump);

        //forward/backward & sideways movement
        if(grounded || airControl)
        {
            //movement wrt camera's lookat
            rb.velocity = transform.forward * movement[0] * speed + transform.right * movement[1] * speed;
        }
        
        //vertical movement
        if(grounded && jump)
        {
            rb.AddForce(new Vector3(0f, jumpForce)); 
            grounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //handle entering in snow/dirt area triggers
        if (other.tag == "Snow_Ground")
        {
            //player will be slower on snow
            speed -= speed * snowMultiplier;
        }
        else if (other.tag == "Dirt_Ground")
        {
            //player will be faster on dirt
            speed += speed * dirtMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // as soon as area is left, reset speed to normal speed
        if (other.tag == "Snow_Ground" || other.tag == "Dirt_Ground")
            speed = moveSpeed;
    }

    */
}
