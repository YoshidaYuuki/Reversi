using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public NetEventResult result;
}

public enum DataType
{
    DATA_TYPE_MATCHING = 0,
    DATA_TYPE_COMMAND = 1
}

public class DataPack
{
    public DataType type;
    public string command;
}


