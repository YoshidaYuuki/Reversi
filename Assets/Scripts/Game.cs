using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private Logic logic;
    private Field field;

    private bool isBlack = true;
    private bool isPlayerBlack;

    private TransportTCP transport;

	void Start () {
        logic = transform.Find("Logic").GetComponent<Logic>();
        field = transform.Find("Field").GetComponent<Field>();

        /* 初期配置 */

        logic.Set(Logic.c_sizeOfField / 2 - 1,  Logic.c_sizeOfField / 2 - 1,    false);
        logic.Set(Logic.c_sizeOfField / 2,      Logic.c_sizeOfField / 2 - 1,    true);
        logic.Set(Logic.c_sizeOfField / 2 - 1,  Logic.c_sizeOfField / 2,        true);
        logic.Set(Logic.c_sizeOfField / 2,      Logic.c_sizeOfField / 2,        false);

        transport = GameObject.Find("Network").GetComponent<TransportTCP>();
        if(transport.IsServer())
        {
            isPlayerBlack = true;
            Debug.Log("あなたは先行です");
        }
        else
        {
            isPlayerBlack = false;
            Debug.Log("あなたは後攻です");
        }
    }

    void Update () {
        if (isBlack == isPlayerBlack)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector2Int v = field.ToIndex(Input.mousePosition);
                if (!logic.CheckSet(v.x, v.y, isBlack))
                {
                    Debug.Log("そこには置けないよ");
                    return;
                }
                byte[] buffer = new byte[2];
                buffer[0] = (byte)v.x;
                buffer[1] = (byte)v.y;

                transport.Send(buffer, buffer.Length);
                isBlack = !isBlack;
                Debug.Log("NOW SIDE = " + (isBlack ? "BLACK" : "WHITE"));
            }
        }
        else
        {
            byte[] buffer = new byte[1400];

            int recvSize = transport.Receive(ref buffer, buffer.Length);
            if (recvSize > 0)
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer);
                Debug.Log("Recv data:" + message);

                logic.CheckSet(buffer[0], buffer[1], isBlack);

                isBlack = !isBlack;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            logic.Clear();
        }
	}
}
