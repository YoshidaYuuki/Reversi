using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButton : MonoBehaviour {

    public GameObject networkObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        byte[] buffer = new byte[256];
        networkObj.GetComponent<TransportTCP>().Receive(ref buffer, buffer.Length);

        Debug.Log(buffer);
	}

    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        networkObj.GetComponent<TransportTCP>().StartServer(25252, 1);
    }
}
