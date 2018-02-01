using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class Matching : MonoBehaviour {

    private TransportTCP transport;
    public GameObject networkObj;

    // Use this for initialization
    void Start () {
        transport = networkObj.GetComponent<TransportTCP>();

	}

    // Update is called once per frame
    void Update()
    {

        byte[] buffer = new byte[1400];

		int recvSize = transport.Receive(ref buffer, buffer.Length);
        if (recvSize > 0)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer);
            Debug.Log("Recv data:" + message);
        }
    }

    public void CreateServer()
    {
        transport.StartServer(25252, 1);
    }

    public void CreateClient()
    {
        transport.Connect("127.0.0.1", 25252);
        Debug.Log("connect");

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("HLSY");
        networkObj.GetComponent<TransportTCP>().Send(buffer, buffer.Length);
    }
}
