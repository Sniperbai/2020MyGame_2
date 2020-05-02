using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour, Openable
{
    Animator door, boss;
    public GameObject bossObj;
    Transform followTarget;

    public GameObject bossDoorSwitch;

    private void Start()
    {
        door = transform.GetComponent<Animator>();
        boss = transform.Find("Boss").GetComponent<Animator>();
        followTarget = transform.Find("followTarget");
    }

    public void OpenDoor()
    {
        //取消对人物的控制
        PlayerInput.instance.SetEnable(false);

        //把相机聚焦在boss身上
        Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(followTarget, 30,2);

        Invoke("PlayBossAnim",2);
    }

    //播放boss的动画
    public void PlayBossAnim()
    {
        //播放动画
        door.Play("boss_door");
        boss.Play("boss_spawn");
    }

    //播放动画完毕
    public void OnOpenDoorOver()
    {
        //隐藏boss动画
        boss.gameObject.SetActive(false);

        //显示 或 创建 BOSS
        bossObj.SetActive(true);

        //把相机聚焦在玩家身上
        Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(GameObject.Find("Player").transform, 46, 2);

        //隐藏BossDoorSwitch
        bossDoorSwitch.gameObject.SetActive(false);


        //恢复对人物的控制
        PlayerInput.instance.SetEnable(true);

        
    }

    public void Open()
    {
        OpenDoor();
    }

    public void Close()
    {
        
    }
}
