using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    //This is attached to the Camera Rig
    //Rotations/Movement need to be done to the rig itself not the camera
    //Need "orbiting" effect around player
    //CameraRig is child to player, hence no need to move it on its own
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity;
    [SerializeField] private float verticalDistance = 1.32f;
    [SerializeField] private float horizontalDistance = 2.68f;
    [SerializeField] private float lookdownAngle = 17.14f;

    private string horizontalAxis = "MouseHorizontal";
    private string verticalAxis = "MouseVertical";
    private Rigidbody rb;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        //update camera to be "x" units away and "y" units away from player
        transform.localPosition = new Vector3(0f, verticalDistance, -horizontalDistance);
        transform.localRotation = Quaternion.Euler(lookdownAngle, 0f, 0f);
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
    }
}
