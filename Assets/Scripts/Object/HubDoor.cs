using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HubDoorStatus
{
    zero = 0,
    one = 1,
    two = 2,
    three = 3,
}

public class HubDoor : MonoBehaviour
{
    public Sprite[] status;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    public void SetStatus(HubDoorStatus s)
    {
        spriteRenderer.sprite = status[(int)s];
    }
}


