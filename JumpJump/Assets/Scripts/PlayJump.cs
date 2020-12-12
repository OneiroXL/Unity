using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayJump : MonoBehaviour
{
    //起始点
    public Transform oriPos;

    //目标点
    public Transform desPos;

    private float vx;
    private float vy;
    private float distance;
    private float g = 9.8f;
    private float t;
    private bool press = false;
    private float pressTime = 0;
    private bool isFly = false;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(oriPos.position, desPos.position);
        vx = distance / 1.0f;
        direction = (desPos.position - oriPos.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            press = true;
        }

        if (press) 
        {
            pressTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            press = false;
            SetTargetPos();
        }

        if (isFly) 
        {
            if (transform.position.y < 3)
            {
                isFly = false;
                transform.position = new Vector3(transform.position.x, 3, transform.position.y);
                t = 0;
                pressTime = 0;
                return;
            }

            t = t + Time.deltaTime;
            vy = g * 1 / 2 - g * t;

            transform.Translate(direction * vx * Time.deltaTime);
            transform.Translate(transform.up * Time.deltaTime * vy);
        }
    }

    void SetTargetPos() 
    {
        distance = pressTime;
        vx = distance / 1.0f;

        isFly = true;
    }
}
