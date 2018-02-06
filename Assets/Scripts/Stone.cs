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


    private Animator turnAnimator;
    private Image image;


    private float turnDelayTime;
    private bool isDelayCounted;


    void Awake()
    {
        image = GetComponent<Image>();
        isBlack = true;

        turnAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDelayCounted)
        {
            turnDelayTime -= Time.deltaTime;
            if (turnDelayTime <= 0)
            {
                isDelayCounted = false;

                turnAnimator.SetTrigger("Turn");
            }
        }
    }

    public void Turn(float delay = 0.0f)
    {
        turnDelayTime = delay;
        isDelayCounted = true;
    }

    public void TurnAnimationEvent()
    {
        isBlack = !isBlack;
    }
}