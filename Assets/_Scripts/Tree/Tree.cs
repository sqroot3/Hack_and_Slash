using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    [SerializeField]
    private bool onFire;
    private GameObject player;
    public GameObject firePit;

    private void Awake()
    {
        firePit.SetActive(false);
    }

    public bool OnFire
    {
        get { return onFire; }
    }

    public void ToggleFire()
    {
        if(onFire)
        {
            onFire = false;
            firePit.SetActive(false);
            Manager.numOfBurningTrees--;
        }
        else
        {
            onFire = true;
            firePit.SetActive(true);
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
