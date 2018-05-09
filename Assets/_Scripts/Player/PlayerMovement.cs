using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

	/* Other */
	public float defaultSpeed;
	public float sprintBoost;	
	public float jumpStrength;
	public float leapVertical;
	public float leapHorizontal;
	public float deathY = -1f;

	
	private Rigidbody rigidbody;
	private float moveVertical;
	private float moveHorizontal;

	private float horizontal;
	private float vertical;

	public static int currentJump = -1;


	/* Ground check stuff */
	public Transform groundCheck;
	public LayerMask groundLayers;

	private bool grounded;
	public static bool checkGrounded;
	private float groundedRadius = .2f;

	/* Animation Stuff */
	private Animator animator;
	private readonly int hashHorizontal = Animator.StringToHash("horizontal");
	private readonly int hashVertical = Animator.StringToHash("vertical");
    private readonly int hashHorizontalJump = Animator.StringToHash("jumpHorizontal");
    private readonly int hashVerticalJump = Animator.StringToHash("jumpVertical");
    private readonly int hashIdle = Animator.StringToHash("idle");
	private readonly int hashSprint = Animator.StringToHash("sprint");
	private readonly int hashJump = Animator.StringToHash("jump");
	private readonly int hashGrounded = Animator.StringToHash("grounded");

    private float jumpHorizontal = 0f;
    private float jumpVertical = 0f;

    public Transform torso;

	void Start () {
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody>();
		checkGrounded = true;
	}

	void FixedUpdate()
	{
		grounded = false;
		if(groundCheck)
		{
			//Player is grounded if sphere overlap hits anything labelled "ground"
            Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundedRadius, groundLayers);
            foreach(Collider c in colliders)
            {
                if (c.gameObject != gameObject)
                    grounded = true;
            }
		}
	}

	void Update () {

		//Check if on death height
		if(transform.position.y <= deathY /*&& !Manager.playerDied*/)
        {
            PlayerHealth health = GetComponent<PlayerHealth>();
            health.OnDeath();
            Debug.Log("Died from a fall");
        }

		//Get input from player
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		bool isSprint = Input.GetButton("Sprint");
		bool isIdle = !(Mathf.Abs(horizontal) > 0f || Mathf.Abs(vertical) > 0f);
		bool isJump = Input.GetButtonDown("Jump");

		//Send to animator
		animator.SetFloat(hashHorizontal, horizontal);
		animator.SetFloat(hashVertical, vertical);
        animator.SetFloat(hashHorizontalJump, jumpHorizontal);
        animator.SetFloat(hashVerticalJump, jumpVertical);
        animator.SetBool(hashIdle, isIdle);
		animator.SetBool(hashSprint, isSprint);

		//Animator should only know grounded before and after executing a jump clip - handled by Animation events
		if(checkGrounded)
			animator.SetBool(hashGrounded, grounded);
		else
			animator.SetBool(hashGrounded, false);
		


		if(isJump && grounded && currentJump == -1) {
			animator.SetTrigger(hashJump);
		}
			

		//Update movement based on animation (needs to take into account camera's lookat)
		
		if(isSprint) {
			moveHorizontal = horizontal * (defaultSpeed + sprintBoost) * Time.deltaTime;
			moveVertical = vertical * (defaultSpeed + sprintBoost) * Time.deltaTime;
		}
		else {
			moveHorizontal = horizontal * defaultSpeed * Time.deltaTime;
			moveVertical = vertical * defaultSpeed * Time.deltaTime;
		}		

		if(currentJump == 2 && !grounded) {
			//fix: not necessarily going "forward" all the time
			//rigidbody.velocity = new Vector3(localRight * moveHorizontal, rigidbody.velocity.y, localForward * leapHorizontal);
			//rigidbody.velocity = localForward * leapHorizontal + localRight * moveHorizontal + transform.up * rigidbody.velocity.y;
			/*
			if(horizontal > 0)
			{
				rigidbody.velocity = this.transform.right * leapHorizontal + this.transform.forward * moveHorizontal + transform.up * rigidbody.velocity.y;
			}
			else if(horizontal < 0)
			{
				rigidbody.velocity = -this.transform.right * leapHorizontal + this.transform.forward * moveHorizontal + transform.up * rigidbody.velocity.y;	
			}
			else {
				rigidbody.velocity = this.transform.forward * leapHorizontal + this.transform.right * moveHorizontal + transform.up * rigidbody.velocity.y;
			}
			*/
			EvaluateJump();
		}
		else {
			rigidbody.velocity = transform.forward * moveVertical + transform.right * moveHorizontal + transform.up * rigidbody.velocity.y;
		}
			
	}

    void LockJump()
    {
        //lock in movement as jump is pressed - commit to jump
        jumpHorizontal = horizontal;
        jumpVertical = vertical;
    }

	public void OnJumpClimax(int type)
	{
		//0 - standing jump
		//1 - walking jump
		//2 - running jump
		switch(type)
		{
			case 0:
				//Debug.Log("Standing jump is at jumping point!");
				rigidbody.velocity = transform.forward * moveVertical + transform.right * moveHorizontal + transform.up * jumpStrength;
				break;
			case 1:
				//Debug.Log("Walking jump is at jumping point!");
				rigidbody.velocity = transform.forward * moveVertical + transform.right * moveHorizontal + transform.up * jumpStrength;
				break;
			case 2:
				//Debug.Log("Running jump is at jumping point!");
				//fix - not necessarily going "forward" all the time
				EvaluateInitialJump();
				Debug.DrawRay(transform.position, rigidbody.velocity, Color.black, 5f);
				Debug.DrawRay(transform.position, this.transform.right, Color.blue, 5f);
				Debug.DrawRay(transform.position, this.transform.up, Color.red, 5f);
				Debug.DrawRay(transform.position, this.transform.up + this.transform.right, Color.green, 5f);
				break;
		}
	}
	
	//Both of the below are also called on a state machine - this is to cover scenarios where the specific events may not be triggered
	public void OnJumpStart(int type)
	{
		//Debug.Log("Called OnJumpStart()!");
		currentJump = type;
		checkGrounded = false;
        LockJump();
    }

	public void OnJumpEnd()
	{
		//Debug.Log("Called OnJumpEnd()!");
		checkGrounded = true;
		currentJump = -1;
        jumpHorizontal = 0;
        jumpVertical = 0;
	}

    void EvaluateJump()
    {
        if (jumpHorizontal > 0)
        {
            //going right
            //Debug.Log("going right");
            rigidbody.velocity = this.transform.right;
            rigidbody.velocity *= leapHorizontal;
        }
        else if (jumpHorizontal < 0)
        {
            //Debug.Log("going left");
            rigidbody.velocity = -this.transform.right;
            rigidbody.velocity *= leapHorizontal;
        }
        else
        {
            //Debug.Log("straight up");
            if (jumpVertical > 0)
                rigidbody.velocity = this.transform.forward * leapHorizontal + this.transform.right + transform.up * rigidbody.velocity.y;
            else if (jumpVertical < 0)
                rigidbody.velocity = -this.transform.forward * leapHorizontal + this.transform.right + transform.up * rigidbody.velocity.y;
        }
    }

	void EvaluateInitialJump()
    {
        if (jumpHorizontal > 0)
        {
            //going right
            //Debug.Log("going right");
            rigidbody.velocity = this.transform.right + this.transform.up;
            rigidbody.velocity *= leapHorizontal;
        }
        else if (jumpHorizontal < 0)
        {
            //Debug.Log("going left");
            rigidbody.velocity = -this.transform.right + this.transform.up;
            rigidbody.velocity *= leapHorizontal;
        }
        else
        {
            //Debug.Log("straight up");
            if (jumpVertical > 0)
                rigidbody.velocity = this.transform.forward * leapHorizontal + Vector3.up * leapVertical;
            else if (jumpVertical < 0)
                rigidbody.velocity = -this.transform.forward * leapHorizontal + Vector3.up * leapVertical;
        }
    }

}