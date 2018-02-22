using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    private Logic logic;
    private Field field;

	[SerializeField]
	private Text countBlack = null;
	[SerializeField]
	private Text countWhite = null;

    private bool isChange = false;
    private bool isBlack = true;
    private bool isPlayerBlack;

    private TransportTCP transport;

    public GameObject turnUI;



	void Start () {
        logic = transform.Find("Logic").GetComponent<Logic>();
        field = transform.Find("Field").GetComponent<Field>();

        /* 初期配置 */

        logic.Set(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2 - 1, false);
        logic.Set(Logic.c_sizeOfField / 2, Logic.c_sizeOfField / 2 - 1, true);
        logic.Set(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2, true);
        logic.Set(Logic.c_sizeOfField / 2, Logic.c_sizeOfField / 2, false);

        turnUI.transform.position = new Vector3(turnUI.transform.position.x, 365.0f, turnUI.transform.position.z);

        transport = GameObject.Find("Network").GetComponent<TransportTCP>();
        if (transport.IsServer())
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

        Operation();

        if (isChange)
        {
            isBlack = !isBlack;
            Debug.Log("NOW SIDE = " + (isBlack ? "BLACK" : "WHITE"));

            isChange = false;

            // ターンUIの位置を変更
            if (isBlack)
            {
                turnUI.transform.position = new Vector3(turnUI.transform.position.x, 365.0f, turnUI.transform.position.z);
            }
            else
            {
                turnUI.transform.position = new Vector3(turnUI.transform.position.x, 150.0f, turnUI.transform.position.z);
            }
        }

        countBlack.text = logic.numOfBlack.ToString();
        countWhite.text = logic.numOfWhite.ToString();
	}

    void Operation()
    {
        if( isBlack == isPlayerBlack )//自分のターンなら
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2Int v = field.ToIndex(Input.mousePosition);
                if (!logic.Check(v.x, v.y, isBlack))
                {
                    return;

                }
                logic.SetStack(isBlack);
                isChange = true;

                byte[] buffer = new byte[2];
                buffer[0] = (byte)v.x;
                buffer[1] = (byte)v.y;

                transport.Send(buffer, buffer.Length);
            }
        }
        else//相手のターンなら
        {
            byte[] buffer = new byte[1400];

            int recvSize = transport.Receive(ref buffer, buffer.Length);
            if (recvSize > 0)
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer);
                Debug.Log("Recv data:" + message);

                if (!logic.Check(buffer[0], buffer[1], isBlack))
                {
                    return;

                }

                logic.SetStack(isBlack);
                isChange = true;
            }
        }
        
    }

    void Pass()
    {
        Debug.Log((isBlack ? "黒" : "白") + "がパスしました");
        isChange = true;
    }
}
