using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OptionPanel : ViewBase
{
    #region 字段
    
    public MenuPanel menuPanel;                         //获取菜单界面
    public OptionAduioSetPanel optionAduioSetPanel;     //获取选项 音量设置界面 
    public OptionOperatorPanel optionOperatorPanel;     //获取选项 操作说明界面

    GameObject btn_audio;                               //选项 音量设置按钮
    GameObject btn_operator;                            //选项 操作说明按钮
    GameObject messagepanel;                            //选项 信息面板
    #endregion

    #region Unity回调

    private void Start()
    {
        btn_audio = transform.Find("bg/btn_audio").gameObject;       
        btn_operator = transform.Find("bg/btn_controller").gameObject;
        messagepanel = transform.Find("bg/MessagePanel").gameObject;
    }

    #endregion

    #region 点击事件

    public void OnAudioClick() {

        HideOrShowOptionPanel(false);
        //显示声音设置界面
        optionAduioSetPanel.Show();

    }

    public void OnOperatorClick() {

        HideOrShowOptionPanel(false);
        //显示操作说明界面
        optionOperatorPanel.Show();
    }

    public void OnBackClick() {

        if (optionAduioSetPanel.IsShow() || optionOperatorPanel.IsShow())
        {

            optionAduioSetPanel.Hide();
            optionOperatorPanel.Hide();

            //显示选项的元素
            HideOrShowOptionPanel(true);

        }
        else {

            //隐藏自己
            this.Hide();

            //显示菜单界面
            menuPanel.Show();

        }
    }

    #endregion

    #region

    //隐藏或显示选项界面下的元素
    void HideOrShowOptionPanel(bool isShow) {

        btn_audio.SetActive(isShow);
        btn_operator.SetActive(isShow);
        messagepanel.SetActive(isShow);

    } 

    #endregion



}


