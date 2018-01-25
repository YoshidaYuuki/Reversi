using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TransportTCP : MonoBehaviour
{

    Socket m_listener;
    Socket m_socket;

    bool m_isServer;
    bool m_isConnected;

    PacketQueue m_sendQueue = new PacketQueue();
    PacketQueue m_recvQueue = new PacketQueue();

    // イベント通知のデリゲート.
    public delegate void EventHandler(NetEventState state);

    private EventHandler m_handler;

    // スレッド実行フラグ.
    protected bool m_threadLoop = false;

    protected Thread m_thread = null;

    private static int s_mtu = 1400;

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

        LaunchThread();

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

        

        LaunchThread();

        return true;
    }

    public void Disconnect()
    {
        m_isConnected = false;

        if (m_socket != null)
        {
            // ソケットのクローズ.
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
            m_socket = null;
        }

        // 切断を通知します.
        if (m_handler != null)
        {
            NetEventState state = new NetEventState();
            state.type = NetEventType.Disconnect;
            state.result = NetEventResult.Success;
            m_handler(state);
        }
    }

    public int Send(byte[] data, int size)
    {
        return m_sendQueue.Enqueue(data, size);
    }

    public int Receive(ref byte[] buffer, int size)
    {
        return m_recvQueue.Dequeue(ref buffer, size);
    }

    public void RegisterEventHandler(EventHandler handler)
    {
        m_handler += handler;
    }

    public void UnregisterEventHandler(EventHandler handler)
    {
        m_handler -= handler;
    }

    // スレッド起動関数.
    bool LaunchThread()
    {
        try
        {
            // Dispatch用のスレッド起動.
            m_threadLoop = true;
            m_thread = new Thread(new ThreadStart(Dispatch));
            m_thread.Start();
        }
        catch
        {
            Debug.Log("Cannot launch thread.");
            return false;
        }

        return true;
    }

    //スレッド側の送受信処理
    public void Dispatch()
    {
        while(m_threadLoop)
        {
            //クライアントからの接続を待ちます
            AcceptClient();
            //クライアントとの送受信を処理します
            if( m_socket != null && m_isConnected == true )
            {
                //送信処理
                DispatchSend();
                //受信処理
                DispatchReceive();
            }
            
        }
    }

    //スレッド側の送信処理
    void DispatchSend()
    {
        //送信処理
        if( m_socket.Poll(0,SelectMode.SelectWrite))
        {
            byte[] buffer = new byte[s_mtu];
            int sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
            while(sendSize > 0)
            {
                m_socket.Send(buffer, sendSize, SocketFlags.None);
                sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
            }
        }
    }

    //スレッドの受信処理
    void DispatchReceive()
    {
        //受信処理
        while(m_socket.Poll(0,SelectMode.SelectRead))
        {
            byte[] buffer = new byte[s_mtu];
            int recvSize = m_socket.Receive(buffer, buffer.Length, SocketFlags.None);
            if( recvSize == 0)
            {
                //切断
                Disconnect();
            }
            else if( recvSize > 0)
            {
                m_recvQueue.Enqueue(buffer, recvSize);
            }
        }
    }

    void AcceptClient()
    {
        if( m_listener != null && m_listener.Poll(0, SelectMode.SelectRead))
        {
            //クライアントから接続がありました
            m_socket = m_listener.Accept();
            m_isConnected = true;
            //イベントを通知します
            if(m_handler != null)
            {
                NetEventState state = new NetEventState();
                state.type = NetEventType.Connect;
                state.result = NetEventResult.Success;
                m_handler(state);
            }
        }
    }


}

