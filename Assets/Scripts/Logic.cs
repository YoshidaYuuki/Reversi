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
    public int numOfBlack { get { return _numOfBlack; } }
    public int numOfWhite { get { return _numOfWhite; } }


    [SerializeField]
    private Field field;
    private State[,] fieldState = new State[c_sizeOfField, c_sizeOfField];


    private Text black, white;
    private int _numOfBlack, _numOfWhite;


    public struct TurnStack
    {
        public int x, y;
        public int distance;
        public TurnStack( int x, int y, int distance ) { this.x = x; this.y = y; this.distance = distance; }
    }
    private Stack<TurnStack> turnStack = new Stack<TurnStack>();


    void Start () {
        black = transform.Find("Black").GetComponent<Text>();
        white = transform.Find("White").GetComponent<Text>();
    }

    void Update () {
        black.text = _numOfBlack.ToString();
        white.text = _numOfWhite.ToString();
	}

    public void Set( int x, int y, bool isBlack, int depth = 0 )
    {
        switch (fieldState[x, y])
        {
            case State.None:
                _numOfBlack = isBlack  ? _numOfBlack + 1 : _numOfBlack;
                _numOfWhite = !isBlack ? _numOfWhite + 1 : _numOfWhite;
                break;
            case State.Black:
                _numOfBlack--;
                _numOfWhite++;
                Debug.Assert(!isBlack, "チェックエラー");
                break;
            case State.White:
                _numOfBlack++;
                _numOfWhite--;
                Debug.Assert(isBlack, "チェックエラー");
                break;
            default:
                Debug.Assert(false, "ありえないステート");
                break;
        }

        fieldState[x, y] = isBlack ? State.Black : State.White;
        field.SetStone(x, y, isBlack, depth);
    }

    public void SetStack( bool isBlack )
    {
        foreach(TurnStack t in turnStack)
        {
            Set(t.x, t.y, isBlack, t.distance);
        }
    }

    public bool Check( int x, int y, bool isBlack )
    {
        bool result = false;

        turnStack.Clear();

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
            turnStack.Push(new TurnStack(x, y, 0));
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
            turnStack.Push(new TurnStack(x, y, count));
            return true;
        }

        return false;
    }

    public void Clear()
    {
        _numOfWhite = _numOfBlack = 0;
        fieldState.Initialize();
        field.ClearStone();
    }
}
