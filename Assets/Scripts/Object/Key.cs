using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public static int keyCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Player)
        {
            //获取一个钥匙
            keyCount++;

            //获取门
            HubDoor hubDoor = GameObject.Find("HubDoor").GetComponent<HubDoor>();


            //更新UITODO

            //销毁钥匙
            //Destroy(gameObject);
            gameObject.SetActive(false);

            if (hubDoor == null)
            {

                throw new System.Exception("未在场景中查询到门");

                //return;
            }

            //人物不可操作
            PlayerInput.instance.SetEnable(false);

            //聚焦到门
            Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(hubDoor.transform,33,1);

            //修改门的状态
            
            Invoke("ChangeHubDoorStatus",1.5f);
            //恢复正常
            Invoke("ResetToNormal", 2f);

        }

        
    }


    public void ChangeHubDoorStatus()
    {
       
        HubDoor hubDoor = GameObject.Find("HubDoor").GetComponent<HubDoor>();

        hubDoor.SetStatus((HubDoorStatus)keyCount);
    }

    //恢复正常状态
    public void ResetToNormal ()
    {
        CameraFollowTarget cameraFollowTarget = Camera.main.GetComponent<CameraFollowTarget>();
        cameraFollowTarget.SetFollowTarget(GameObject.Find("Player/followTarget").transform, cameraFollowTarget.defualtView, 1);

        //人物可操作
        PlayerInput.instance.SetEnable(true);

        //销毁钥匙
        Destroy(gameObject);
    }
}


