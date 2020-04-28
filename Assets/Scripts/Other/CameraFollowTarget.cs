using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollowTarget : MonoBehaviour
{
    #region 字段

    public Transform target;    //要跟随的游戏物体

    public Vector2 rangeMin;    //跟随的最小范围
    public Vector2 rangeMax;    //跟随的最大范围

    public bool isFollowWithTime = false;  //使用一定的时间移动过去 
    public float delayTime;     //延迟的时间
    public float timer;         //计时
    public Vector3 startPos;     //开始的位置

    public bool isChangeView;   //是否修改View

    [HideInInspector]
    public float defualtView;
    public float currentView;
    public float targetView;
    private Transform defaultTarget;

    Camera camera;

    #endregion

    private void Start()
    {
        camera = transform.GetComponent<Camera>();
        defualtView = camera.fieldOfView;
    }

    private void Update()
    {
        Follow(); 
    }

    public void Follow()
    {
        if (target == null)
        {
            Debug.LogWarning(transform.name + "要跟随的目标为空！");
            return;
        }

        Vector3 targetPos ;

        if (isFollowWithTime)
        {
            timer += Time.deltaTime;
            targetPos = Vector3.Lerp(startPos, target.position, timer / delayTime);
            targetPos.z = transform.position.z;

            if (isChangeView)
            {
                camera.fieldOfView = Mathf.Lerp(currentView, targetView, timer / delayTime);
            }

            if (timer / delayTime > 1)
            {
                isChangeView = false;
                isFollowWithTime = false;
            }
        }
        else
        {
            targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        

        transform.position = LimitPos(targetPos);

    }

    public void SetFollowTarget(Transform target)
    {
        isFollowWithTime = false;
        this.target = target;
    }

    public void SetFollowTarget(Transform target, float time)
    {
        this.target = target;
        isFollowWithTime = true;
        timer = 0;
        delayTime = time;
        startPos = transform.position;
    }

    public void SetFollowTarget(Transform target,float view ,float time)
    {
        isChangeView = true;
        targetView = view;
        currentView = camera.fieldOfView;
        SetFollowTarget(target,time);
    }

    //当物体下落调用 过一段时间 恢复
    public void SetFollowTarget(GameObject target,int time)
    {
        defaultTarget = this.target;   //对当前相机的跟随的目标做一个保存
        SetFollowTarget( target.transform,time );

        Invoke("ResetFollowTarget",3);
    }

    public void ResetFollowTarget()
    {
        Debug.Log("ResetFollowTarget");
        SetFollowTarget(defaultTarget, 1);
        //恢复玩家的操作
        PlayerInput.instance.SetEnable(true);
    }
    public Vector3 LimitPos(Vector3 targetPos)
    {
        if (targetPos.x < rangeMin.x)
        {
            targetPos.x = rangeMin.x;
        }

        if (targetPos.y < rangeMin.y)
        {
            targetPos.y = rangeMin.y;
        }

        if (targetPos.x > rangeMax.x)
        {
            targetPos.x = rangeMax.x;
        }

        if (targetPos.y > rangeMax.y)
        {
            targetPos.y = rangeMax.y;
        }

        return targetPos;
    }
}

