using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TipStyle
{
    Style1,  //位于屏幕下方的提示
    Style2,  //全黑的样式
    Style3,  //游戏结束的时候显示
}

public class TipMessagePanel : SingletonView<TipMessagePanel>
{
    #region 字段

    GameObject style1Obj;
    GameObject style2Obj;
    GameObject style3Obj;

    #endregion

    #region Unity的回调

    protected override void Awake()
    {
        base.Awake();
        style1Obj = transform.Find("Style1").gameObject;
        style1Obj.SetActive(false);

        style2Obj = transform.Find("Style2").gameObject;
        style2Obj.SetActive(false);

        style3Obj = transform.Find("Style3").gameObject;
        style3Obj.SetActive(false);
    }


    #endregion

    #region 方法

    public void ShowTip(string content,TipStyle tipStyle)
    {
        switch (tipStyle)
        {
            case TipStyle.Style1:

                style1Obj.SetActive(true);

                style1Obj.transform.Find("Content").GetComponent<Text>().text = content;

                break;
            case TipStyle.Style2:
                style2Obj.SetActive(true);
                //1.5秒后隐藏
                Invoke("HideTipStyle2",1.5f);
                break;
            case TipStyle.Style3:
                style3Obj.SetActive(true);
                //2秒之后自动隐藏
                Invoke("HideTipStyle3", 2f);
                break;
        }


    }

    public void HideTipStyle2()
    {
        HideTip(TipStyle.Style2);
    }

    public void HideTipStyle3()
    {
        HideTip(TipStyle.Style3);


    }

    public void HideTip(TipStyle tipStyle)
    {

        switch (tipStyle)
        {
            case TipStyle.Style1:

                style1Obj.SetActive(false);

                break;
            case TipStyle.Style2:
                style2Obj.SetActive(false);
                break;

            case TipStyle.Style3:
                style3Obj.SetActive(false);
                break;
        }

    }

    #endregion
}

