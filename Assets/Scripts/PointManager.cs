using UnityEngine;
using System.Collections;

public class PointManager : MonoBehaviour {
    private int point = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Set(int num)
    {
        point = num;
    }

    public int Get()
    {
        return point;
    }
}
