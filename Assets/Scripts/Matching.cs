using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matching : MonoBehaviour {

    public GameObject serverPrefab;
    public GameObject clietnPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateServer()
    {
        Instantiate(serverPrefab);
    }

    public void CreateClient()
    {
        Instantiate(serverPrefab);
    }
}
