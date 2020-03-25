using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPlatform : MonoBehaviour
{
    
    int targetLayer;    //下落的层
    PlatformEffector2D platformEffector;

    private void Start()
    {
        platformEffector = transform.GetComponent<PlatformEffector2D>();
    }

    //穿过这个平台
    public void Fall(GameObject target)
    {
        //把这个层给取消
        targetLayer = 1 << target.layer;
        platformEffector.colliderMask &= ~targetLayer;

        //还需要恢复
        Invoke("ResetLayer",0.5f);
    }

    public void ResetLayer()
    {
        platformEffector.colliderMask |= targetLayer;
    }

}


