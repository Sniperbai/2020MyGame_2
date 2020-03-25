using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    public static SceneController _instance;

    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SceneController();
            }

            return _instance;
        }

    }

    public  AsyncOperation currentLoadOperation;    //加载的信息

    //加载场景
    public void LoadScene(int target)
    {
        //加载界面
        if (SceneLoadPanel._instance == null)
        {
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/View/LoadScenePanel"));
        }

        
        //加载
        currentLoadOperation  = SceneManager.LoadSceneAsync(target);

        //显示界面
        SceneLoadPanel._instance.UpdateProcess(currentLoadOperation);
    }

    public void LoadScene(int target, Action<AsyncOperation> onComplete)
    {
        //加载界面
        if (SceneLoadPanel._instance == null)
        {
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/View/LoadScenePanel"));
        }


        //加载
        currentLoadOperation = SceneManager.LoadSceneAsync(target);

        //显示界面
        SceneLoadPanel._instance.UpdateProcess(currentLoadOperation);
        currentLoadOperation.completed += onComplete;
    }

    public void LoadScene(int target, string objName, string posName)
    {
        LoadScene(target, (asyncOperation) => {
            GameObject gameObject = GameObject.Find(objName);
            GameObject posObject = GameObject. Find(posName);

            gameObject.transform.position = posObject.transform.position;
        });
    }
}


