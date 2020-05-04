using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerProjectile : BulletBase
{
    public float speed = 10f;

    public override void Awake()
    {
        base.Awake();
        Destroy(gameObject, 5f);
    }

    public void SetDirection( Vector3 direction)
    {
        rigidbody2d.velocity = direction * speed;
    }
}
