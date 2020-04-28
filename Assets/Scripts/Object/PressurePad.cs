using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : SwitchBase
{
    GameObject on_light;     //打开状态的光源
    int objCount;

    public GameObject[] openableTargets;

    protected override void Start()
    {
        base.Start();
        on_light = transform.Find("on_light").gameObject;
        on_light.SetActive(false);
        //等于0 说明没有设置数据 把以前设置的数据设置给他（只是为了收容）
        if (openableTargets.Length == 0)
        {
            openableTargets = new GameObject[] { openableTarget };
        }
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

    public override void Open()
    {
        if (openableTarget == null)
        {
            base.Open();
        }
        else
        {
            switchStatus = SwitchStatus.Open;
            spriteRenderer.sprite = statusSprites[(int)switchStatus];     //把图片设置成打开的状态
            OnOpen();
            //触发对应的事件
            if (openableTargets == null) return;

            for (int i = 0; i < openableTargets.Length; i++)
            {
                if (openableTargets[i].GetComponent<Openable>() != null)
                {
                    openableTargets[i].GetComponent<Openable>().Open();
                }
            }
            
        }
    }

    public override void Close()
    {
        if (openableTarget == null)
        {
            base.Close();
        }
        else
        {
            switchStatus = SwitchStatus.Close;
            spriteRenderer.sprite = statusSprites[(int)switchStatus];     //把图片设置成打开的状态
            OnClose();
            //触发对应的事件
            if (openableTargets == null) return;
            for(int i = 0; i < openableTargets.Length;i++)
            if (openableTargets[i].GetComponent<Openable>() != null)
            {
                openableTargets[i].GetComponent<Openable>().Close();
            }
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
