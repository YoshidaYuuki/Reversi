using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// オセロの石
public class Stone : MonoBehaviour
{

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


    private Spin spinAnimation;
    private Image image;


    private Vector3 axis;
    private float turnDelayTime;
    private bool isDelayCounted;


    void Awake()
    {
        image = GetComponent<Image>();
        isBlack = true;

        spinAnimation = GetComponent<Spin>();
    }

    void Update()
    {
        if (isDelayCounted)
        {
            turnDelayTime -= Time.deltaTime;
            if (turnDelayTime <= 0)
            {
                isDelayCounted = false;

                spinAnimation.Play(axis);
            }
        }
    }

    public void Turn(Vector3 axis, float delay = 0.0f)
    {
        this.axis = axis;
        turnDelayTime = delay;
        isDelayCounted = true;
    }

    public void TurnAnimationEvent()
    {
        isBlack = !isBlack;
    }
}