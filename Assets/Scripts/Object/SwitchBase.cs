using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Openable
{
    //打开的时候调用
    void Open();
    //关闭的时候调用
    void Close();
}

public enum SwitchStatus
{
    Close = 0,     //关着的状态
    Open = 1       //开着的状态
}

public class SwitchBase : MonoBehaviour
{
    public SwitchStatus switchStatus = SwitchStatus.Close;
    public Sprite[] statusSprites;
    protected SpriteRenderer spriteRenderer;

    public GameObject openableTarget;      //要打开的目标

    protected virtual void Start()
    {
        //初始化
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = statusSprites[(int)switchStatus];
    }

    public virtual void Open()
    {
        switchStatus = SwitchStatus.Open;
        spriteRenderer.sprite = statusSprites[(int)switchStatus];     //把图片设置成打开的状态
        OnOpen();
        //触发对应的事件
        if (openableTarget == null) return;
        if( openableTarget.GetComponent<Openable>() != null )
        {
            openableTarget.GetComponent<Openable>().Open();
        }
        
    }

    public virtual void Close()
    {
        switchStatus = SwitchStatus.Close;
        spriteRenderer.sprite = statusSprites[(int)switchStatus];     //把图片设置成打开的状态
        OnClose();
        //触发对应的事件
        if (openableTarget == null) return;
        if (openableTarget.GetComponent<Openable>() != null)
        {
            openableTarget.GetComponent<Openable>().Close();
        }
        
    }

    public virtual void OnOpen() { }

    public virtual void OnClose() { }
}
