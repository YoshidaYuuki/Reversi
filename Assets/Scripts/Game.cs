using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private Logic logic;
    private Field field;


    private bool isBlack = true;
    public GameObject turnUI;       // どちらのターンか表すUI

	void Start () {
        logic = transform.Find("Logic").GetComponent<Logic>();
        field = transform.Find("Field").GetComponent<Field>();

        /* 初期配置 */

        logic.Set(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2 - 1, false);
        logic.Set(Logic.c_sizeOfField / 2, Logic.c_sizeOfField / 2 - 1, true);
        logic.Set(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2, true);
        logic.Set(Logic.c_sizeOfField / 2, Logic.c_sizeOfField / 2, false);
        turnUI.transform.position = new Vector3(turnUI.transform.position.x, 365.0f, turnUI.transform.position.z);
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int v = field.ToIndex(Input.mousePosition);
            if (!logic.Check(v.x, v.y, isBlack))
            {
                Debug.Log("そこには置けないよ");
                return;
            }

            logic.SetStack(isBlack);

            isBlack = !isBlack;
            Debug.Log("B/W = " + logic.numOfBlack + "/" + logic.numOfWhite);
            Debug.Log("NOW SIDE = " + (isBlack ? "BLACK" : "WHITE"));

            // ターンUIの位置を変更
            if(isBlack)
            {
                turnUI.transform.position = new Vector3(turnUI.transform.position.x, 365.0f, turnUI.transform.position.z);
            }
            else
            {
                turnUI.transform.position = new Vector3(turnUI.transform.position.x, 150.0f, turnUI.transform.position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            logic.Clear();
        }
	}
}
