using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spin : MonoBehaviour {

    [SerializeField]
    float angle = 90.0f;
    [SerializeField]
    float distance = 30.0f;
    [SerializeField]
    float speed = 1.0f;

    Stone stone;

    Vector3 position;
    Vector3 axis;
    int sign;
    float time;
    bool isAnimated;

	void Start () {
        stone = GetComponent<Stone>();
	}
	
	void Update () {

        if (isAnimated)
        {
            time += Time.deltaTime * speed;

            if (sign > 0 && time >= 0.5f)
            {
                sign = -1;
                stone.TurnAnimationEvent();
            }

            if (time >= 1.0f)
            {
                FinalizeAnimation();
                isAnimated = false;

                return;
            }

            Animation();
        }
    }

    void InitializeAnimation()
    {
        transform.localRotation = Quaternion.identity;
        position = transform.localPosition;
    }

    void Animation()
    {
        float t = Time.deltaTime * sign * speed * 2;
        transform.Rotate(axis, angle * t);
        transform.Translate(Vector3.up * distance * t);
    }

    void FinalizeAnimation()
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = position;
    }

    public void Play(Vector3 axis)
    {
        this.axis = Vector3.Cross(axis, Vector3.forward).normalized;
        time = 0;
        sign = 1;
        isAnimated = true;

        InitializeAnimation();
    }
}
