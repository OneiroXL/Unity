using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayJump : MonoBehaviour
{
    public Camera myCamera;

    //起始点
    public Transform oriPos;

    //目标点
    public Transform desPos;

    public GameObject box;

    private float vx;
    private float vy;
    private float distance;
    private float g = 9.8f;
    private float t;
    private bool press = false;
    private float pressTime = 0;
    private float up = 0;
    private bool isFly = false;
    private bool isSuccess = false;
    private Vector3 direction;
    private Vector3 preMid;
    private Vector3 curMid;
    private Vector3 cameraTargetPos;


    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(oriPos.position, desPos.position);
        vx = distance / 1.0f;
        direction = (desPos.position - oriPos.position).normalized;
        desPos.tag = "target";

        preMid = (oriPos.position + desPos.position) / 2.0f;
        curMid = preMid;
        cameraTargetPos = myCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isSuccess = false;
            press = true;
        }

        if (press)
        {
            if (oriPos.position.y >= 0.5)
            {
                pressTime += Time.deltaTime;
                oriPos.Translate(transform.up * -1 * pressTime * 0.005f);
                transform.Translate(transform.up * -1 * pressTime * 0.005f);
            }

        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            press = false;
            SetTargetPos();
        }

        if (!press) 
        {
            if (oriPos.position.y <= 0.5)
            {
                oriPos.Translate(transform.up * pressTime);
            } 
        }

        if (isFly)
        {
            if (transform.position.y < 2.5)
            {
                isFly = false;
                transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
                t = 0;
                pressTime = 0;
                return;
            }

            t = t + Time.deltaTime;
            vy = g * 1 / 2 - g * t * 1.6f;

            transform.Translate(direction * vx * Time.deltaTime);
            transform.Translate(transform.up * Time.deltaTime * vy);

            //旋转
            //transform.GetChild(0).Rotate(0, 0, 360 * Time.deltaTime);

        }
        myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraTargetPos, Time.deltaTime);
    }

    void FixedUpdate()
    {
       
    }

    void SetTargetPos() 
    {
        distance = pressTime * 5;
        vx = distance / 1.0f;

        isFly = true;
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "target")
        {
            float distance = Random.Range(2, 6);
            int dir = Random.Range(0, 2);

            Vector3 dirNext;

            if (dir == 0)
            {
                dirNext = new Vector3(0, 0, -1);
            }
            else
            {
                dirNext = new Vector3(-1, 0, 0);
            }

            Vector3 tarPos = desPos.position + dirNext * distance;

            GameObject nextCube = GameObject.Instantiate(box, tarPos, Quaternion.identity);
            oriPos = desPos;
            desPos = nextCube.transform;
            oriPos.tag = "empty";
            desPos.tag = "target";

            direction = (desPos.position - oriPos.position).normalized;

            preMid = curMid;
            curMid = desPos.position + oriPos.position / 2.0f;
            Vector3 temDir = curMid - preMid;
            cameraTargetPos = myCamera.transform.position + temDir;

        }
    }
}
