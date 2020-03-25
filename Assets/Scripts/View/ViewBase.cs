using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{
    //显示界面
    public virtual void Show()
    {

        transform.gameObject.SetActive(true);
    }

    //隐藏界面
    public virtual void Hide()
    {

        transform.gameObject.SetActive(false);
    }

    //判断是否显示
    public bool IsShow()
    {

        return gameObject.activeSelf;

    }
}


