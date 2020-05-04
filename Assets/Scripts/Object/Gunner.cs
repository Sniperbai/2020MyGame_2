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

    public GameObject bossBullet2;
    public Transform bullet2Pos;

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
        int attackType = 2;
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
                //Debug.Log("触发第二种攻击！");

                //创建一个子弹
                GameObject bullet2 = GameObject.Instantiate(bossBullet2);

                //设置子弹的位置
                bullet2.transform.position = bullet2Pos.position;

                //向玩家抛一个子弹
                float g = Mathf.Abs(Physics2D.gravity.y) * bullet2.transform.GetComponent<Rigidbody2D>().gravityScale;

                float v0 = 8;    //数值向上的初速度
                float t0 = v0 / g;
                float y0 = 0.5f * g * t0 * t0;
                float v = 0;

                float x = attackTarget.transform.position.x - transform.position.x + Random.Range(-1.5f, 1.5f);

                if (transform.position.y + y0 > attackTarget.transform.position.y)
                {
                    //计算子弹需要的初速度
                    // y = 0.5 * a * t * t

                    float y = transform.position.y - attackTarget.transform.position.y + y0;

                    float t = Mathf.Sqrt((y * 2) / g) + t0;
                    v = x / t;
                }
                else if (transform.position.y + y0 < attackTarget.transform.position.y)
                {
                    float y = attackTarget.transform.position.y - transform.position.y;
                    float t = Mathf.Sqrt((y * 2) / g);

                    v0 = g * t;
                    v = x / t;
                }

                bullet2.GetComponent<Gunner2Bullet>().SetSpeed(new Vector2(v,v0));

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

        //隐藏攻击1的红线
        attack1Line.gameObject.SetActive(false);

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
