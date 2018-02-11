using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private Logic logic;
    private Field field;


    private bool isBlack = true;


	void Start () {
        logic = transform.Find("Logic").GetComponent<Logic>();
        field = transform.Find("Field").GetComponent<Field>();

        /* 初期配置 */

        logic.Set(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2 - 1, false);
        logic.Set(Logic.c_sizeOfField / 2, Logic.c_sizeOfField / 2 - 1, true);
        logic.Set(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2, true);
        logic.Set(Logic.c_sizeOfField / 2, Logic.c_sizeOfField / 2, false);
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
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            logic.Clear();
        }
	}
}
