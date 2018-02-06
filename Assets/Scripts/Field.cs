using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// フィールド
public class Field : MonoBehaviour {

    public float width { get { return rectTransform.sizeDelta.x; } }
    public float height { get { return rectTransform.sizeDelta.y; } }


    [SerializeField]
    private GameObject stonePrefab;


    private GameObject[,] stone = new GameObject[Logic.c_sizeOfField, Logic.c_sizeOfField];


    private RectTransform rectTransform;
    private Vector2 canvasSize;


    [SerializeField]
    private float delayFrame;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CanvasScaler cs = transform.parent.GetComponent<CanvasScaler>();
        canvasSize = cs.referenceResolution;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetStone( int x, int y, bool isBlack, int depth = 0 )
    {
        GameObject g = stone[x, y];

        if (g == null)
        {
            g = Instantiate(stonePrefab, this.transform);
            g.GetComponent<RectTransform>().localPosition = ToWorld(x, y);
    	    g.GetComponent<Stone>().isBlack = isBlack;
	        stone[x, y] = g;
        }
        else
        {
            g.GetComponent<Stone>().Turn(depth * delayFrame);
        }
    }

    public void ClearStone()
    {
        stone.Initialize();

        foreach( Transform t in transform )
        {
            Destroy(t.gameObject);
        }
    }

    // マウス座標->オセロインデックス
    public Vector2Int ToIndex(Vector3 mousePosition)
    {
        Vector3 p = mousePosition;

        /* スクリーン->ワールド->ビューポート->Canvas変換 */

        p = Camera.main.ScreenToWorldPoint(p + Camera.main.transform.forward * 10);
        p = Camera.main.WorldToViewportPoint(p);
        p.x = (p.x * canvasSize.x) - (canvasSize.x * 0.5f);
        p.y = (p.y * canvasSize.y) - (canvasSize.y * 0.5f);

        Vector2Int vi = new Vector2Int();

        // 座標をマス目の座標へ
        vi.x = Mathf.FloorToInt(p.x / (width / Logic.c_sizeOfField));
        vi.y = Mathf.FloorToInt(p.y / (height / Logic.c_sizeOfField));

        // 左上を[0, 0]になるようにインデックスを変換
        vi.x = vi.x + (Logic.c_sizeOfField / 2);
        vi.y = -vi.y + (Logic.c_sizeOfField / 2 - 1);
        return vi;
    }

    // インデックス->ワールド座標
    public Vector3 ToWorld(int x, int y)
    {
        Vector3 v = new Vector3();

        // 配列用のインデックスを変換
        v.x = x - (Logic.c_sizeOfField / 2) + 0.5f;
        v.y = -y + (Logic.c_sizeOfField / 2 - 1) + 0.5f;

        // ワールド座標へ変換
        v.x = v.x * (width / Logic.c_sizeOfField);
        v.y = v.y * (height / Logic.c_sizeOfField);

        return v;
    }
}