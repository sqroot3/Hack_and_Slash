using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    [SerializeField]
    private bool onFire;
    [SerializeField]
    private GameObject player;

    public bool OnFire
    {
        get { return onFire; }
    }

    void ToggleFire()
    {
        if(onFire)
        {
            onFire = false;
            Manager.numOfBurningTrees--;
        }
        else
        {
            onFire = true;
            Manager.numOfBurningTrees++;
        }
    }
    
    public float distance(Vector3 src)
    {
        return Vector3.Distance(transform.position, src);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ToggleFire();
        }
    }
}
