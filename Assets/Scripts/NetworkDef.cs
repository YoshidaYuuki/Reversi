using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDef : MonoBehaviour {

    public enum NetEventType
    {
        Connect = 0,    //接続イベント
        Disconnect,     //切断イベント
        SendError,      //送信エラー
        ReciveError     //受信エラー
    }

    public enum NetEventResult
    {
        Failure = -1,
        Success = 0,
    }

    public class NetEventState
    {
        public NetEventType type;
        public NetEventResult resultl;
    }


    public delegate void EventHandler(NetEventState state);
}
