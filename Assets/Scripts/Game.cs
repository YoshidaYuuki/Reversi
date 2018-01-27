using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private Logic logic;
    private Field field;

    private bool black;

	void Start () {
        logic = transform.Find("Logic").GetComponent<Logic>();
        field = transform.Find("Field").GetComponent<Field>();

        /* 初期配置 */

        //SetStone(Logic.c_sizeOfField / 2 + 1, Logic.c_sizeOfField / 2 - 1, true);
        //SetStone(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2 + 1, false);
        //SetStone(Logic.c_sizeOfField / 2 + 1, Logic.c_sizeOfField / 2 + 1, true);
        //SetStone(Logic.c_sizeOfField / 2 - 1, Logic.c_sizeOfField / 2 - 1, false);
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int v = field.ToIndex(Input.mousePosition);
            logic.Set(v.x, v.y, black);
        }

        if (Input.GetMouseButtonDown(1))
        {
            black = !black;
            Debug.Log("NOW SIDE = " + ( black ? "BLACK" : "WHITE" ));
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            logic.Clear();
        }
	}
}
