using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{

    public  AsyncOperation currentLoadOperation;    //加载的信息

    //加载场景
    public void LoadScene(int target)
    {        
        //加载
        currentLoadOperation  = SceneManager.LoadSceneAsync(target);

        //显示界面
        SceneLoadPanel.Instance.UpdateProcess(currentLoadOperation);
    }

    public void LoadScene(int target, Action<AsyncOperation> onComplete)
    {
        //加载
        currentLoadOperation = SceneManager.LoadSceneAsync(target);

        //显示界面
        SceneLoadPanel.Instance.UpdateProcess(currentLoadOperation);
        currentLoadOperation.completed += onComplete;
    }

    public void LoadScene(int target, string objName, string posName, bool isFlipX = false )
    {
        LoadScene(target, (asyncOperation) => {
            GameObject gameObject = GameObject.Find(objName);
            GameObject posObject = GameObject. Find(posName);

            gameObject.transform.position = posObject.transform.position;
            gameObject.transform.rotation = posObject.transform.rotation;

            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = isFlipX;
            }
        });
    }
}


