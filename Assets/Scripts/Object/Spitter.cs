using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : EnemyBase
{
    #region 字段
    GameObject bulletProfab;

    public Transform bulletSpawnPos;

    //public float speed;

    //Transform startCheckPos;     //检测前方是否可以移动

    //public bool isCanMove = true;

    //float idleTimer;

    #endregion

    #region Unity回调

    //protected override void Start()
    //{
    //    base.Start();

    //    //startCheckPos = transform.Find("startCheckPos");
    //}

    protected override void Update()
    {
        base.Update();

        //CheckIsCanMove();     //检测是否可以移动

        UpdateDirection();    //更新敌人的方向

        //CheckIsCanAttack();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == TagConst.Player)
        {
            damage.OnDamage(collision.gameObject);
        }
    }

    #endregion

    #region 方法

    //检查是否可以移动
    //public void CheckIsCanMove()
    //{
    //    RaycastHit2D raycastHit2D = Physics2D.Raycast(startCheckPos.position, Vector2.down, 1, 1 << 8);

    //    Debug.DrawLine(startCheckPos.position, startCheckPos.position + Vector3.down, Color.red);

    //    isCanMove = raycastHit2D;
    //}

    public override void OnUpdateStatus()
    {
        base.OnUpdateStatus();

        switch (enemyStatus)
        {
            case EnemyStatus.Idle:

                SetSpeedX(0);
                //idleTimer += Time.deltaTime;
                //if (idleTimer > 2)
                //{
                //    idleTimer = 0;
                //    enemyStatus = EnemyStatus.Walk;
                //}

                break;

            //case EnemyStatus.Walk:

            //    SetSpeedX(speed);
            //    if (!isCanMove)
            //    {
            //        speed = -speed;
            //        startCheckPos.localPosition = new Vector3(-startCheckPos.localPosition.x, startCheckPos.localPosition.y, startCheckPos.localPosition.z);
            //    }
            //    animator.SetBool("isWalk", true);

            //    break;

            case EnemyStatus.Attack:

                //设置奔跑动画
                animator.SetBool("isAttack", true);

                break;

            //case EnemyStatus.Run:

            //    //设置奔跑动画
            //    animator.SetBool("isRun", true);
            //    //跑向要攻击的目标
            //    if (isCanMove)
            //    {
            //        if (attackTarget.position.x - transform.position.x > 0)
            //        {
            //            //在右边
            //            speed = Mathf.Abs(speed);
            //            startCheckPos.localPosition = new Vector3(Mathf.Abs(startCheckPos.localPosition.x), startCheckPos.localPosition.y, startCheckPos.localPosition.z);
            //        }
            //        else
            //        {
            //            //在左边
            //            speed = -Mathf.Abs(speed);
            //            startCheckPos.localPosition = new Vector3(-Mathf.Abs(startCheckPos.localPosition.x), startCheckPos.localPosition.y, startCheckPos.localPosition.z);
            //        }
            //        SetSpeedX(speed);
            //    }
            //    else
            //    {
            //        SetSpeedX(0);
            //        enemyStatus = EnemyStatus.Idle;
            //    }
            //    break;
            case EnemyStatus.Dead:

                animator.SetBool("isDead", true);

                break;
        }

        //if (enemyStatus != EnemyStatus.Walk)
        //{
        //    animator.SetBool("isWalk", false);
        //}
        //if (enemyStatus != EnemyStatus.Run)
        //{
        //    animator.SetBool("isRun", false);
        //}
        if (enemyStatus != EnemyStatus.Attack)
        {
            animator.SetBool("isAttack", false);
        }
    }

    //public override void OnDead(string resetPos)
    //{
    //    SetSpeedX(0);
    //    base.OnDead(resetPos);
    //}

    //更新敌人的方向

    public void UpdateDirection()
    {
        if (attackTarget.position.x - transform.position.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (attackTarget.position.x - transform.position.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    //是不是能够攻击了
    //public void CheckIsCanAttack()
    //{
    //    if (enemyStatus == EnemyStatus.Attack)
    //    {
    //        if (attackTarget.position.y > transform.position.y + 2)
    //        {
    //            enemyStatus = EnemyStatus.Idle;
    //        }
    //    }
    //}

    //进行攻击

    public override void OnAttack()
    {
        //base.OnAttack();

        //创建一个子弹
        if (bulletProfab == null)
        {
            bulletProfab = Resources.Load<GameObject>("Prefabs/Object/AcidBubbles");
        }

        Debug.Log("创建一个子弹！");

        GameObject bullet = GameObject.Instantiate(bulletProfab);

        bullet.transform.position = bulletSpawnPos.position;

        float g = Mathf.Abs(Physics2D.gravity.y) * bullet.transform.GetComponent<Rigidbody2D>().gravityScale;

        float v0 = 8;    //数值向上的初速度
        float t0 = v0 / g;
        float y0 = 0.5f * g * t0 * t0;
        float v = 0;

        float x = attackTarget.position.x - transform.position.x + Random.Range(-1.5f, 1.5f);

        if (transform.position.y + y0 > attackTarget.position.y)
        {
            //计算子弹需要的初速度
            // y = 0.5 * a * t * t

            float y = transform.position.y - attackTarget.position.y + y0;

            float t = Mathf.Sqrt((y * 2) / g) + t0;
            v = x / t;
        }
        else if (transform.position.y + y0 < attackTarget.position.y)
        {
            float y = attackTarget.position.y - transform.position.y;
            float t = Mathf.Sqrt((y * 2) / g);

            v0 = g * t;
            v = x / t;
        }
        
        bullet.GetComponent<AcidBubbles>().SetSpeed(new Vector2 (v,v0));
    }

    #endregion
}
