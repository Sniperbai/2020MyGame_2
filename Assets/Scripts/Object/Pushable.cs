using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Pushable : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    private void Start()
    {

        rigidbody2d = transform.GetComponent<Rigidbody2D>();
    }

    public void Move(float x)
    {
        Move(new Vector2(x, rigidbody2d.velocity.y));
    }

    public void Move( Vector2 velocity)
    {
        rigidbody2d.velocity = velocity;
    }
}
