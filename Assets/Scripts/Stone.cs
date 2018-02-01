using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// オセロの石
public class Stone : MonoBehaviour {

    private bool _isBlack;
    public bool isBlack
    {
        set
        {
            _isBlack = value;
            image.sprite = stoneImage[isBlack ? 0 : 1];
        }
        get
        {
            return _isBlack;
        }
    }

    [SerializeField]
    private Sprite[] stoneImage = new Sprite[2];

    private Image image;

	void Awake () {
        image = GetComponent<Image>();
        isBlack = true;
	}
	
	void Update () {
		
	}
}
