using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Dialog pDialog;

	// Use this for initialization
	void Start () {
        pDialog = GetComponent<Dialog>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            //Debug.Log("collided with wall!");
            StartCoroutine(pDialog.WriteMessage(0.1f, 4f, "You shouldn't have come here..", true));
        }
    }

}
