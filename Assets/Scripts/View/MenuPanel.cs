using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuPanel : ViewBase
{
    #region 字段
    
    public OptionPanel optionPanel;               //获取选项界面

    #endregion

    private void Start()
    {
        AudioManager.Instance.PlayMusic("Audio/Music/MusicGameplay");
    }

    #region 点击事件

    public void OnStartGameClick() {
        //播放按钮点击的音乐
        AudioManager.Instance.PlaySound("Audio/UI/MenuButton01");

        //跳转到第一关场景
        SceneController.Instance.LoadScene(1);

    }

    public void OnOptionClick() {

        //播放按钮点击的音乐
        AudioManager.Instance.PlaySound("Audio/UI/MenuButton01");

        //隐藏自己
        this.Hide();

        //显示选项界面
        optionPanel.Show();

    }

    public void OnExitClick() {

        //播放按钮点击的音乐
        AudioManager.Instance.PlaySound("Audio/UI/MenuButton01");

        //if (Application.isEditor)
        //{

        //    EditorApplication.isPlaying = false;     //编辑器

        //}
        //else {

        //    Application.Quit();                     //非编辑器

        //}



        //UNITY的预处理指令
#if UNITY_EDITOR                                      //判断是否是在编辑器模式
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

#endregion

}


