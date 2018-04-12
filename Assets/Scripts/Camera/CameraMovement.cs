using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    //This is attached to the Camera Rig
    //Rotations/Movement need to be done to the rig itself not the camera
    //Need "orbiting" effect around player
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity;
    [SerializeField] private float minPlayerDistance = 2.80f;

    private string horizontalAxis = "MouseHorizontal";
    private string verticalAxis = "MouseVertical";
    private Rigidbody rb;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //get mouse input, rotate camera by that amount
        float rotateInput = Input.GetAxis(horizontalAxis);
        //Debug.Log(rotateInput);
        if(Mathf.Abs(rotateInput) > 0.1f)
        {
            Rotate(rotateInput / sensitivity );
        }
	}

    public void Rotate(float angle)
    {
        //Orbit around player by "angle" amount
        player.Rotate(new Vector3(0f, angle,0f));
        //transform.RotateAround(player.position, Vector3.up, -angle);
        
    }

    public void Move(Vector3 velocity)
    {
        /*
        Debug.Log("Distance to player: " + Vector3.Distance(transform.position, player.position));
        rb.velocity = velocity;
        */
        
    }
}
