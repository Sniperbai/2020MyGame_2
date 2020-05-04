using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner2Bullet : BulletBase
{
    private void Start()
    {
        Destroy(gameObject, 5);   //5s之后自动销毁
    }

    public void SetSpeed(Vector2 vector2)
    {
        rigidbody2d.velocity = vector2;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.tag != TagConst.Player)
        {
            return;
        }
        base.OnCollisionEnter2D(collision);
    }
}
