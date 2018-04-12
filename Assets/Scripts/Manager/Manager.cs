using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static List<Tree> trees = new List<Tree>();

	// Use this for initialization
	void Start () {
        InitializeTrees();
        Debug.Log("Initialized trees!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeTrees()
    {
        GameObject[] result = GameObject.FindGameObjectsWithTag("Tree");
        for(int i = 0; i < result.Length; ++i)
        {
            Tree t = result[i].GetComponent<Tree>();
            trees.Add(t);
        }
    }
}
