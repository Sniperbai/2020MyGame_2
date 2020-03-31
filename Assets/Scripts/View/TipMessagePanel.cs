using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TipStyle
{
    Style1,  //位于屏幕下方的提示
    Style2,  //全黑的样式
}

public class TipMessagePanel : SingletonView<TipMessagePanel>
{
    #region 字段

    GameObject style1Obj;
    GameObject style2Obj;

    #endregion

    #region Unity的回调

    protected override void Awake()
    {
        base.Awake();
        style1Obj = transform.Find("Style1").gameObject;
        style1Obj.SetActive(false);

        style2Obj = transform.Find("Style2").gameObject;
        style2Obj.SetActive(false);
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
        }


    }

    public void HideTipStyle2()
    {
        HideTip(TipStyle.Style2);
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
        }

    }

    #endregion
}

