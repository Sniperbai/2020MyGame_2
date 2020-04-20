using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformOpenable : MovingPlatform, Openable
{
    bool isOpen;               //是不是打开的状态

    public void Close()
    {
        isOpen = false;
    }

    public void Open()
    {
        isOpen = true;
    }

    protected override void Update()
    {
        if (isOpen)
        {
            base.Update();
        }  
    }
}
