using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunnerStatus
{
    Idle,
    Attack,
    Disable,
    Dead

}

public class Gunner : MonoBehaviour
{
    Animator animator;

    Damageable bossAble;    //boss自身的血量
    Damageable defenceAble; //能量罩的血量

    public GunnerStatus currentStatus = GunnerStatus.Idle;

    int defaultDefenceHP;

    #region  Unity的回调

    private void Start()
    {
        animator = transform.GetComponent<Animator>();

        bossAble = transform.GetComponent<Damageable>();
        bossAble.OnHurt += this.OnBossHurt;
        bossAble.OnDead += this.OnBossDead;

        defenceAble = transform.Find("GunnerShield").GetComponent<Damageable>();
        defenceAble.OnHurt += this.OnDefenceHurt;
        defenceAble.OnDead += this.OnDefenceDead;

        defaultDefenceHP = defenceAble.health;
    }

    private void Update()
    {
        //更新状态
        OnUpdateStatus();

    }

    #endregion

    public void OnUpdateStatus()
    {
        switch (currentStatus)
        {
            case GunnerStatus.Idle:
                break;
            case GunnerStatus.Attack:
                break;
            case GunnerStatus.Disable:
                animator.SetBool("isDisable", true);
                break;
            case GunnerStatus.Dead:
                animator.SetBool("isDead", true);
                break;
        }
        if (currentStatus != GunnerStatus.Disable)
        {
            animator.SetBool("isDisable", false);
        }
    }

    #region  受伤的相关方法

    public void OnBossHurt(HurtType hurtType, string resetPos)
    {
        if (defenceAble.health > 0)
        {
            defenceAble.TakeDamage(1, hurtType, resetPos);
            bossAble.health++;
            return;
        }
    }

    public void OnBossDead(string resetPos)
    {
        currentStatus = GunnerStatus.Dead;
        animator.SetTrigger("Trigger");
    }

    public void OnDefenceHurt(HurtType hurtType, string resetPos)
    {

    }

    public void OnDefenceDead(string resetPos)
    {
        Debug.Log("防御罩被打碎了");
        currentStatus = GunnerStatus.Disable;
        animator.SetTrigger("Trigger");
        Invoke("ResetDefence", 5);
    }

    public void ResetDefence()
    {
        currentStatus = GunnerStatus.Idle;
        defenceAble.health = defaultDefenceHP;
    }

    #endregion


}
