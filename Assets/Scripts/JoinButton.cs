using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinButton : MonoBehaviour {

    public GameObject networkObj;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        networkObj.GetComponent<TransportTCP>().Connect("127.0.0.1", 25252);

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("HLSY");
        networkObj.GetComponent<TransportTCP>().Send(buffer,buffer.Length);
    }
}
