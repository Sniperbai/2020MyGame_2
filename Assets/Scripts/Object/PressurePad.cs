using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : SwitchBase
{
    GameObject on_light;     //打开状态的光源
    int objCount;

    protected override void Start()
    {
        base.Start();
        on_light = transform.Find("on_light").gameObject;
        on_light.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != TagConst.ground && collision.name != "attackRange")
        {
            Open();
            objCount++;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objCount--;
        if (objCount <= 0)
        {
            Close();
        }      
    }

    public override void OnOpen()
    {
        base.OnOpen();
        on_light.SetActive(true);
    }

    public override void OnClose()
    {
        base.OnClose();
        on_light.SetActive(false);
    }
}
