using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// フィールド
public class Field : MonoBehaviour {

    public RectTransform stone;

    private RectTransform rectTransform;
    private Vector2 canvasSize;

    private Logic logic;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CanvasScaler cs = transform.parent.GetComponent<CanvasScaler>();
        canvasSize = cs.referenceResolution;

        logic = transform.Find("Logic").GetComponent<Logic>();
    }
	
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            stone.localPosition = ToWorld(ToIndex(Input.mousePosition));
        }
    }

    // マウス座標->オセロインデックス
    Vector2Int ToIndex( Vector3 mousePosition )
    {
        Vector3 p = mousePosition;

        /* スクリーン->ワールド->ビューポート->Canvas変換 */

        p = Camera.main.ScreenToWorldPoint(p + Camera.main.transform.forward * 10);
        p = Camera.main.WorldToViewportPoint(p);
        p.x = (p.x * canvasSize.x) - (canvasSize.x * 0.5f);
        p.y = (p.y * canvasSize.y) - (canvasSize.y * 0.5f);

        Vector2Int vi = new Vector2Int();

        // 座標をマス目の座標へ
        vi.x = Mathf.FloorToInt(p.x / (rectTransform.sizeDelta.x / Logic.c_sizeOfField));
        vi.y = Mathf.FloorToInt(p.y / (rectTransform.sizeDelta.y / Logic.c_sizeOfField));

        // 左上を[0, 0]になるようにインデックスを変換
        vi.x = vi.x + (Logic.c_sizeOfField / 2);
        vi.y = -vi.y + (Logic.c_sizeOfField / 2 - 1);
        return vi;
    }
    
    // インデックス->ワールド座標
    Vector3 ToWorld( Vector2Int index )
    {
        Vector3 v = new Vector3();

        // 配列用のインデックスを変換
        v.x = index.x - (Logic.c_sizeOfField / 2) + 0.5f;
        v.y = -index.y + (Logic.c_sizeOfField / 2 - 1) + 0.5f;

        // ワールド座標へ変換
        v.x = v.x * (rectTransform.sizeDelta.x / Logic.c_sizeOfField);
        v.y = v.y * (rectTransform.sizeDelta.y / Logic.c_sizeOfField);

        return v;
    }
}