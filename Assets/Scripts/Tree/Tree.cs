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
        set { onFire = value; }
    }

    void ToggleFire()
    {
        onFire = !onFire;
    }
    
    public float distance(Vector3 src)
    {
        return Vector3.Distance(transform.position, src);
    }
}
