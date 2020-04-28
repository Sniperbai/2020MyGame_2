using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyUnityEvent : UnityEvent<GameObject,int> { }

public enum PushableStatus
{
    Idle,
    Fall
}

[RequireComponent(typeof(Rigidbody2D))]

public class Pushable : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    public PushableStatus currentStatus = PushableStatus.Idle;

    public Transform[] startCheckPos;

    public bool isGrounded;   //是不是在地面上

    public MyUnityEvent OnStartFall;

    public GameObject cameraFollw;      //当箱子下落 相机要聚焦的物体

    public int time;

    bool isCanFall = true;

    bool isPush = false;  // 是不是正在推 

    private void Start()
    {

        rigidbody2d = transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGround();
        // 如果不是推的状态 x 方向速度为 0 
        if (!isPush)
        {
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }
    }

    public void Move(float x)
    {
        if (!isGrounded) { return; }
        isPush = true;
        Move(new Vector2(x, rigidbody2d.velocity.y));
    }

    public void Move( Vector2 velocity)
    {
        rigidbody2d.velocity = velocity;
        Invoke("ResetIsPush", 0.1f);
    }

    public void CheckGround()
    {
        for (int i = 0; i < startCheckPos.Length; i++)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(startCheckPos[i].position, Vector3.down, 1.3f, 1 << 8);
            Debug.DrawLine(startCheckPos[i].position,startCheckPos[i].position+Vector3.down*1.3f,Color.red);

            isGrounded = raycastHit2D;
            if (isGrounded)
            {
                currentStatus = PushableStatus.Idle;
                break;
            }
        }

        if (!isGrounded && currentStatus == PushableStatus.Idle)
        {
            //开始下落
            Debug.Log("开始下落了");

            if (OnStartFall!= null && isCanFall)  
            {
                OnStartFall.Invoke(cameraFollw, time);
                isCanFall = false;

                Invoke("ResetIsCanFall", 3);
            }
            
            currentStatus = PushableStatus.Fall;
        }
    }

    public void ResetIsCanFall()
    {
        isCanFall = true;
    }
    public void ResetIsPush()
    {
        isPush = false;
    }
}
