using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class MyIpAddress : MonoBehaviour {

    private TransportTCP transport;
    public GameObject networkObj;
    IPAddress address;

    // Use this for initialization
    void Start()
    {
        transport = networkObj.GetComponent<TransportTCP>();
        
        //Debug.Log(address.ToString());
    }

	
	// Update is called once per frame
	void Update () {
        transport.GetIPAddress(ref address);
        this.GetComponent<Text>().text = address.ToString();
    }
}
