using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput :MonoBehaviour
{
    #region 字段

    public bool isEnable = true;   //当前是不是可操作的状态

    #endregion

    public static PlayerInput instance;

    #region 输入事件

    public InputButton Pause = new InputButton(KeyCode.Escape);    //暂停
    public InputButton Attack = new InputButton(KeyCode.K);        //攻击
    public InputButton Shoot = new InputButton(KeyCode.O);         //射击
    public InputButton Jump = new InputButton(KeyCode.Space);          //跳跃

    public InputAxis Horizontal = new InputAxis(KeyCode.A,KeyCode.D);   //水平移动
    public InputAxis Vertical = new InputAxis(KeyCode.S,KeyCode.W);     //垂直移动

    #endregion

    public void Awake()
    {
        if ( instance != null )
        {
            throw new System.Exception("PlayerInput 存在多个对象！");
        }

        instance = this;
    }

    private void Update()
    {

        if (isEnable)
        {
            Pause.Get();
            Attack.Get();
            Shoot.Get();
            Jump.Get();

            Horizontal.Get();
            Vertical.Get();
        }

    }

    public void SetEnable(bool isCanUse)
    {
        this.isEnable = isCanUse;
    }
}

