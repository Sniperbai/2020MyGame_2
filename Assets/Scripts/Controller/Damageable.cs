using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Damageable : MonoBehaviour
{
    public int health;    //生命值
    int defaultHealth;    //默认的生命值
    public Action<HurtType,string> OnHurt;  //
    public Action<string> OnDead;  //

    public bool isEnable = true ;

    private void Start()
    {
        defaultHealth = health;
    }

    public void Enanble()
    {
        isEnable = true;
    }

    public void Disable()
    {
        isEnable = false;
    }

    //受伤
    public void TakeDamage(int damage,HurtType hurtType,string ResetPos)
    {
        if (isEnable == false) { return; }  //不可用的状态
        if (health <= 0){ return; }          //已经死亡了

        //血量要减少
        health--;
        if (health == 0)
        {
            //死掉了
            if (OnDead != null)
            {
                OnDead(ResetPos);
            }
            
            
        }
        else
        {
            //受伤了
            if (OnHurt != null )
            {
                OnHurt(hurtType, ResetPos);
            }
            
        }


        //播放受伤动画...

        //判断血量是不是为0


    }

    //重置
    public void ResetHealth()
    {
        this.health = defaultHealth;
        Enanble();
    }
}


