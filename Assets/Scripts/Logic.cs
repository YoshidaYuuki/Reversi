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
        if (x < 0 || x >= c_sizeOfField) return;
        if (y < 0 || y >= c_sizeOfField) return;

        fieldState[x, y] = isBlack ? State.Black : State.White;
        field.SetStone(x, y, isBlack);
    }

    public void Clear()
    {
        fieldState.Initialize();
        field.ClearStone();
    }
}
