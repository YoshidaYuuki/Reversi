using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// アルゴリズム
public class Logic : MonoBehaviour {

    public enum State
    {
        None,
        Black,
        White
    };

    [SerializeField]
    public const int c_sizeOfField = 8;
    [SerializeField]
    private Field field;

    private State[,] fieldState = new State[c_sizeOfField, c_sizeOfField];

    void Start () {
    }

    void Update () {
		
	}

    public void Set( int x, int y, bool isBlack )
    {
        fieldState[x, y] = isBlack ? State.Black : State.White;
        field.SetStone(x, y, isBlack);
    }

    public bool CheckSet( int x, int y, bool isBlack )
    {
        bool result = false;

        if (x < 0 || x >= c_sizeOfField) return false;
        if (y < 0 || y >= c_sizeOfField) return false;
        if (fieldState[x, y] != State.None) return false;

        result |= Turn(x, y, +0, -1, 0, isBlack); // 上
        result |= Turn(x, y, +1, -1, 0, isBlack); // 右上
        result |= Turn(x, y, +1, +0, 0, isBlack); // 右
        result |= Turn(x, y, +1, +1, 0, isBlack); // 右下
        result |= Turn(x, y, +0, +1, 0, isBlack); // 下
        result |= Turn(x, y, -1, +1, 0, isBlack); // 左下
        result |= Turn(x, y, -1, +0, 0, isBlack); // 左
        result |= Turn(x, y, -1, -1, 0, isBlack); // 左上

        if (result)
        {
            Set(x, y, isBlack);
        }
        return result;
    }

    public bool Turn( int x, int y, int ofsX, int ofsY, int count, bool isBlack )
    {
        x += ofsX;
        y += ofsY;

        if (x < 0 || x >= c_sizeOfField) return false;
        if (y < 0 || y >= c_sizeOfField) return false;

        State s = fieldState[x, y];
        if (s == State.None)
        {
            return false;
        }

        if (s == (isBlack ? State.Black : State.White))
        {
            return count > 0;
        }

        if (Turn(x, y, ofsX, ofsY, count + 1, isBlack) == true)
        {
            Set(x, y, isBlack);
            return true;
        }

        return false;
    }

    public void Clear()
    {
        fieldState.Initialize();
        field.ClearStone();
    }
}
