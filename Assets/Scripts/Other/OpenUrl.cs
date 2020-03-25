using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    public string url;//要打开网址 iphone平台链接不能有汉字

    public void OperUrl()
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        Application.OpenURL(url);
    }
}


