using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBubbles : BulletBase
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,10);
    }

    public void SetSpeed(Vector2 velocity)
    {
        rigidbody2d.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
