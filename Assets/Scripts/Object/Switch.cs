using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 开关
public class Switch : SwitchBase
{
    GameObject on_light;            //开着状态的灯光

    protected override void Start()
    {
        base.Start();
        on_light = transform.Find("on_light").gameObject;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Bullet || collision.tag == TagConst.pushable)
        {
            //打开
            Open();
            
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();
        on_light.SetActive(true);                                     //打开灯光

        //把检测关掉
        transform.GetComponent<BoxCollider2D>().enabled = false;
    }

    

}
