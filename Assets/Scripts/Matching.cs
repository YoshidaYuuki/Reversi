using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Matching : MonoBehaviour {

    private TransportTCP transport;
    public GameObject networkObj;
    public GameObject textBox;

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

            switch(message)
            {
                case "connect":
                    {
                        
                        break;
                    }
            }

            Debug.Log(message);
            if(networkObj.GetComponent<TransportTCP>().IsConnected() == true)
            {
                SceneManager.LoadScene("Game");
                Debug.Log("JoinGame");
            }
        }
    }

    public void CreateServer()
    {
        transport.StartServer(25252, 1);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("connect");
        networkObj.GetComponent<TransportTCP>().Send(buffer, buffer.Length);
    }

    public void CreateClient()
    {
        transport.Connect(textBox.GetComponent<Text>().text, 25252);
        Debug.Log("connectadd" + textBox.GetComponent<Text>().text);

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("connect");
        networkObj.GetComponent<TransportTCP>().Send(buffer, buffer.Length);
    }
}
