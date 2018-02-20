using UnityEngine;
using System.Collections.Generic;

public class Point_UI_Manager : MonoBehaviour {

    [SerializeField]
    List<GameObject> numberPointList;

    private PointManager pointManager;

    // Use this for initialization
    void Start () {
        pointManager = GetComponent<PointManager>();
    }
	
	// Update is called once per frame
	void Update () {
        int point = pointManager.Get();
        int workPoint;

        for(int i = 0; i < numberPointList.Count; i++)
        {
            workPoint = point % 10;

            Point number = numberPointList[i].GetComponent<Point>();
            number.SetNumber(workPoint);

            point = point / 10;
        }
    }
}
