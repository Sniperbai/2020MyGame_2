using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MovingType
{
   Horizontal,  //水平方向
   Vertical     //垂直方向
}

public class MovingPlatform : MonoBehaviour
{
    //开始的位置
    public Vector3 startPos;

    //结束的位置
    public Vector3 endPos;

    public MovingType movingType = MovingType.Horizontal;

    //当前移动的速度
    public float speed;

    bool isMoveToEnd = true;      //是不是向结束位置移动

    Rigidbody2D rigidbody2d;

    public ContactFilter2D contactFilter;
    ContactPoint2D[] contactPoint = new ContactPoint2D[10];

    private void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
    }

    
    public void FollowObjects()
    {
        //获取平台上的游戏物体
        int count = rigidbody2d.GetContacts(contactFilter, contactPoint);

        for (int i = 0; i < count; i++)
        {
            if (movingType == MovingType.Horizontal)
            {
                contactPoint[i].rigidbody.velocity += new Vector2(isMoveToEnd ? speed : -speed, 0);
            }
            else
            {
                //contactPoint[i].rigidbody.velocity += new Vector2(0, isMoveToEnd ? speed : -speed);
            }
            
        }
    }

    private void LateUpdate()
    {
        FollowObjects();
    }

    public void Update()
    {
        if (isMoveToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

            if (transform.position == endPos)
            {
                isMoveToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (transform.position == startPos)
            {
                isMoveToEnd = true;
            }
        }

        
        
    }
}

