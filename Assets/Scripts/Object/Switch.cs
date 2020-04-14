using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchStatus
{
    Close = 0,     //关着的状态
    Open = 1       //开着的状态
}

// 开关
public class Switch : MonoBehaviour
{
    public SwitchStatus switchStatus = SwitchStatus.Close;
    public Sprite[] statusSprites;
    SpriteRenderer spriteRenderer;
    GameObject on_light;            //开着状态的灯光

    public Door door;

    void Start()
    {
        //初始化
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = statusSprites[(int)switchStatus];
        on_light = transform.Find("on_light").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Bullet)
        {
            switchStatus = SwitchStatus.Open;
            spriteRenderer.sprite = statusSprites[(int)switchStatus];     //把图片设置成打开的状态
            on_light.SetActive(true);                                     //打开灯光
            //把对应的门打开
            if (door != null)
            {
                door.Open();
            }
            

            //把检测关掉
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }
    }



}
