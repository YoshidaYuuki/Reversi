using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButton : MonoBehaviour {

    public GameObject matchingObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        matchingObj.GetComponent<Matching>().CreateServer();
    }
}
