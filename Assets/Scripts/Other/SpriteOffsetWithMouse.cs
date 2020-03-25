using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteOffsetWithMouse : MonoBehaviour
{
    public float spriteScaler;

    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + (Input.mousePosition * spriteScaler);
    }
}


