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

        Operation();

        if (isChange)
        {
            isBlack = !isBlack;
            Debug.Log("NOW SIDE = " + (isBlack ? "BLACK" : "WHITE"));

            isChange = false;
        }
        
        countBlack.text = logic.numOfBlack.ToString();
        countWhite.text = logic.numOfWhite.ToString();
	}

    void Operation()
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
        }
    }

    void Pass()
    {
        Debug.Log((isBlack ? "黒" : "白") + "がパスしました");
        isChange = true;
    }
}
