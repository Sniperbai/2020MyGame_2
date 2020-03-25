using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadPanel : SingletonView<SceneLoadPanel>
{
    #region 字段

    Slider slider_process;
    AsyncOperation currentLoadScene;   //当前加载的场景

    #endregion

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        slider_process = transform.Find("bg/Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        UpdateProcess(currentLoadScene.progress);
    }

    //更新进度
    public void UpdateProcess(float process)
    {
        this.Show();
        this.slider_process.value = process;

        if (process >= 1)  //说明加载完成
        {
            //this.Hide();
            Invoke("Hide",1);
        }
    }

    //更新加载场景的进度
    public void UpdateProcess(AsyncOperation asyncOperation)
    {
        this.Show();
        currentLoadScene = asyncOperation;
    }

    public override void Hide()
    {
        base.Hide();
        currentLoadScene = null;
    }

}

