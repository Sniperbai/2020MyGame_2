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

    public float attackTime = 3;

    public GameObject bossBullet1;

    public Transform bullet1Pos;

    GameObject attackTarget;
    LineRenderer attack1Line;
    List<Vector3> attack1LinePosition = new List<Vector3>();

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

        attackTarget = GameObject.Find("Player");

        attack1Line = transform.Find("attack1Line").GetComponent<LineRenderer>();

        StartAttack();
    }

    private void Update()
    {
        //更新状态
        OnUpdateStatus();
        //更新攻击的线
        UpdateAttack1Line();
    }

    #endregion

    #region 方法

    //开始攻击
    public void StartAttack()
    {
        InvokeRepeating("Attack",1,attackTime);
    }

    public void Attack()
    {
        //int attackType = Random.Range(1, 4);
        int attackType = 1;
        switch (attackType)
        {
            case 1:
                attack1Line.transform.gameObject.SetActive(true);
                //attack1Line
                break;
        }
        if (attackType != 1)
        {
            attack1Line.transform.gameObject.SetActive(false);
        }
        animator.SetFloat("attackType", attackType);
        animator.SetTrigger("attack");

    }

    //执行攻击
    public void AttackExc(int type)
    {
        switch (type)
        {
            case 1:        //向玩家发射一个子弹
                GameObject bulletObj1 = GameObject.Instantiate(bossBullet1);
                bulletObj1.transform.position = bullet1Pos.position;
                bulletObj1.GetComponent<GunnerProjectile>().SetDirection(((attackTarget.transform.position + Vector3.up) - bullet1Pos.position).normalized);
                break;
            case 2:
                break;
            case 3:
                break;

        }
    }

    //攻击结束
    public void AttackEnd()
    {

    }

    //停止攻击
    public void StopAttack()
    {
        CancelInvoke("Attack");
    }

    public void UpdateAttack1Line()
    {
        attack1LinePosition.Clear();
        attack1LinePosition.Add(bullet1Pos.position);
        attack1LinePosition.Add(bullet1Pos.position+(attackTarget.transform.position+Vector3.up - bullet1Pos.position).normalized * 20);
        attack1Line.positionCount = attack1LinePosition.Count;
        attack1Line.SetPositions(attack1LinePosition.ToArray());

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

        // 停止攻击
        StopAttack();
    }

    public void ResetDefence()
    {
        currentStatus = GunnerStatus.Idle;
        defenceAble.health = defaultDefenceHP;

        //开始攻击
        StartAttack();
    }

    #endregion


}
