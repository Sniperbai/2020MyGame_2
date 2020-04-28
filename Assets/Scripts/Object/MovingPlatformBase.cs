using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingType
{
    Horizontal,  //水平方向
    Vertical     //垂直方向
}

public class MovingPlatformBase : MonoBehaviour
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

    List<Rigidbody2D> contacts = new List<Rigidbody2D>();

    protected virtual void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
    }


    public void FollowObjects()
    {
        contacts.Clear();
        //获取平台上的游戏物体
        int count = rigidbody2d.GetContacts(contactFilter, contactPoint);

        for (int i = 0; i < count; i++)
        {
            if (!contacts.Contains(contactPoint[i].rigidbody))
            {
                contacts.Add(contactPoint[i].rigidbody);
            }    
        }

        if (movingType == MovingType.Horizontal)
        {
            foreach (Rigidbody2D rigid in contacts)
            {
                if (startPos.x < endPos.x)  // 开始向右移动
                {
                    rigid.velocity += new Vector2(isMoveToEnd ? speed : -speed, 0);
                }
                else                        // 开始向左移动
                {
                    rigid.velocity += new Vector2(isMoveToEnd ? -speed : speed, 0);
                }
            }
            
        }
    }

    protected virtual void LateUpdate()
    {
        FollowObjects();
    }

    protected virtual void Update()
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
