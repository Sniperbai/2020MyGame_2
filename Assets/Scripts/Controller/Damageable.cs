using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Damageable : MonoBehaviour
{
    public int health;    //生命值

    public Action OnHurt;  //
    public Action OnDead;

    //受伤
    public void TakeDamage(int damage)
    {
        //血量要减少
        health--;
        if (health == 0)
        {
            //死掉了
            if (OnDead != null)
            {
                OnDead();
            }
            
            
        }
        else
        {
            //受伤了
            if (OnHurt != null )
            {
                OnHurt();
            }
            
        }


        //播放受伤动画...

        //判断血量是不是为0


    }
}


