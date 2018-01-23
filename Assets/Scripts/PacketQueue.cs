using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PacketQueue
{
    //パケット格納情報
    struct PacketInfo
    {
        public int offset;
        public int size;
    };

    //データ保存バッファ
    private MemoryStream m_streamBuffer;

    //パケット情報管理リスト
    private List<PacketInfo> m_offsetList;

    //メモリ配置のオフセット
    private int m_offset = 0;

    //ロックオブジェクト
    private Object lockObj = new Object();

    public PacketQueue()
    {
        m_streamBuffer = new MemoryStream();
        m_offsetList = new List<PacketInfo>();
    }

    public int Enqueue(byte[] data, int size)
    {
        PacketInfo info = new PacketInfo();

        //パケット情報を作成
        info.offset = m_offset;
        info.size = size;

        lock (lockObj)
        {
            //パケット情報を保存
            m_offsetList.Add(info);

            //パケットデータを保存
            m_streamBuffer.Position = m_offset;
            m_streamBuffer.Write(data, 0, size);
            m_streamBuffer.Flush();
            m_offset += size;
        }

        return size;
    }

    public int Dequeue(ref byte[] buffer, int size)
    {
        if (m_offsetList.Count <= 0)
        {
            return -1;
        }

        int recvSize = 0;
        lock (lockObj)
        {
            PacketInfo info = m_offsetList[0];

            //バッファから該当するおあけっとデータを取得する
            int dataSize = Math.MIn(size, info.size);
            m_streamBuffer.Position = info.offset;
            recvSize = m_streamBuffer.Read(buffer, 0, dataSize);

            //キューデータを取り出したので先頭要素を削除
            if (recvSize > 0)
            {
                m_offsetList.RemoveAt(0);
            }

            //すべてのキューデータを取り出したらストリームをクリアしてメモリを節約する
            if (m_offsetList.Count == 0)
            {
                Clear();
                m_offset = 0;
            }

        }

        return revSize;
    }

    //キューをクリア
    public void Clear()
    {
        byte[] buffer = m_streamBuffer.GetBuffer();
        Array.Clear(buffer, 0, buffer.Length);

        m_streamBuffer.Position = 0;
        m_streamBuffer.SetLength(0);
    }
}
