using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene :MonoBehaviour
{
    public int targetScene;

    public string posName;     //游戏物体的位置的名称
    public string objName;    //需要设置位置的游戏物体
    public bool isFlip;       //是不是需要翻转

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("切换场景！" + targetScene);

            if (string.IsNullOrEmpty(posName) || string.IsNullOrEmpty(objName))
            {
                SceneController.Instance.LoadScene(targetScene);
            }
            else
            {
                SceneController.Instance.LoadScene( targetScene, objName, posName,isFlip);
            }

            
        }
    }
}


