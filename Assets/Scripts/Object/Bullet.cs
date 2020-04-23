using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BulletBase
{
    #region 字段

    public float speed;

    #endregion

    #region  Unity回调

    public void SetDirection(bool isRight)
    {
        spriteRenderer.flipX = !isRight;
        rigidbody2d.velocity = new Vector2(isRight?speed:-speed,0);
    }

    #endregion



}
