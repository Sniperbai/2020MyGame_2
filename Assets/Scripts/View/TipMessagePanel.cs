using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TipStyle
{
    Style1,  //位于屏幕下方的提示
}

public class TipMessagePanel : SingletonView<TipMessagePanel>
{
    #region 字段

    GameObject style1Obj;


    #endregion

    #region Unity的回调

    private void Start()
    {
        style1Obj = transform.Find("Style1").gameObject;

        style1Obj.SetActive(false);
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
        }


    }

    public void HideTip(TipStyle tipStyle)
    {

        switch (tipStyle)
        {
            case TipStyle.Style1:

                style1Obj.SetActive(false);

                break;
        }

    }

    #endregion
}

