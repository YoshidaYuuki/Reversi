using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TranspotrTCP : MonoBehaviour
{

    Socket m_listener;
    Socket m_socket;

    bool m_isServer;
    bool m_isConnected;

    PacketQueue m_sendQueue = new PacketQueue();
    PacketQueue m_recvQueue = new PacketQueue();

    //通知デリゲート
    private NetworkDef.EventHandler m_handler;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool StartServer(int port, int connectionNum)
    {
        //リスニングソケット生成
        m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_listener.Bind(new IPEndPoint(IPAddress.Any, port));
        m_listener.Listen(connectionNum);
        m_isServer = true;

        return true;
    }

    public void StopServer()
    {
        //リスニングソケットの破棄
        m_listener.Close();
        m_listener = null;
        m_isServer = false;
    }

    public bool Connect(string address, int port)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.NoDelay = true;
        m_socket.Connect(address, port);
        m_socket.SendBufferSize = 0;
        m_isConnected = true;

        return true;
    }

    public bool Disconnect()
    {
        //ソケットの破棄
        m_socket.Shutdown(SocketShutdown.Both);
        m_socket.Close();
        m_socket = null;

        m_isConnected = false;

        return true;
    }

    public int Send(byte[] data, int size)
    {
        return m_sendQueue.Enqueue(data, size);
    }

    public int Receive(ref byte[] buffer, int size)
    {
        return m_recvQueue.Dequeue(ref buffer, size);
    }

    public void RegisterEventHandler(NetworkDef.EventHandler handler)
    {
        m_handler += handler;
    }
}

