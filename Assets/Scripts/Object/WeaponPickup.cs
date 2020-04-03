using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite haveWeapon, noWeapon;

    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        InitData();


    }

    public void InitData()
    {
        //获取当前有没有武器
        Data<bool> data = (Data<bool>)DataManager.Instance.GetData(DataConst.is_have_weapon);
        if (data != null && data.value1)
        {
            spriteRenderer.sprite = noWeapon;
        }
        else
        {
            spriteRenderer.sprite = haveWeapon;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Player)
        {
            spriteRenderer.sprite = noWeapon;

            //对数据进行保存
            Data<bool> data = new Data<bool>();
            data.value1 = true;
            DataManager.Instance.SaveData(DataConst.is_have_weapon, data);
        }
        
    }


}
