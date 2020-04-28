using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//敌人
public class Chomper : EnemyBase
{
    #region 字段


    public float speed;

    Transform startCheckPos;     //检测前方是否可以移动

    public bool isCanMove = true;

    float idleTimer;

    #endregion

    #region Unity回调

    protected override void Start()
    {
        base.Start();

        startCheckPos = transform.Find("startCheckPos");
    }

    protected override void Update()
    {
        base.Update();

        CheckIsCanMove();     //检测是否可以移动

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
    public void CheckIsCanMove()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(startCheckPos.position, Vector2.down, 1, 1 << 8);
        RaycastHit2D raycast_forward = Physics2D.Raycast(startCheckPos.position,spriteRenderer.flipX? Vector2.left : Vector2.right, 0.5f, 1 << 8);

        Debug.DrawLine(startCheckPos.position, startCheckPos.position + Vector3.down,Color.red);
        Debug.DrawLine(startCheckPos.position, startCheckPos.position + (spriteRenderer.flipX ? Vector3.left : Vector3.right)*0.5f, Color.red);

        if (raycast_forward)
        {
            isCanMove = false;
        }
        else
        {
            isCanMove = raycastHit2D;
        }
    }

    public override void OnUpdateStatus()
    {
        base.OnUpdateStatus();

        switch (enemyStatus)
        {
            case EnemyStatus.Idle:

                SetSpeedX(0);
                idleTimer += Time.deltaTime;
                if (idleTimer > 2)
                {
                    idleTimer = 0;
                    enemyStatus = EnemyStatus.Walk;
                }

                break;

            case EnemyStatus.Walk:

                SetSpeedX(speed);
                if (!isCanMove)
                {
                    speed = -speed;
                    startCheckPos.localPosition = new Vector3(-startCheckPos.localPosition.x, startCheckPos.localPosition.y, startCheckPos.localPosition.z);
                }
                animator.SetBool("isWalk", true);

                break;

            case EnemyStatus.Attack:

                //设置奔跑动画
                animator.SetBool("isAttack", true);

                break;

            case EnemyStatus.Run:

                //设置奔跑动画
                animator.SetBool("isRun", true);
                //跑向要攻击的目标 (能够移动 并且 要在可监听的范围之内)
                if (isCanMove && Vector3.Distance(transform.position,attackTarget.position) <= listenRange)
                {
                    if (attackTarget.position.x - transform.position.x > 0)
                    {
                        //在右边
                        speed = Mathf.Abs(speed);
                        startCheckPos.localPosition = new Vector3(Mathf.Abs(startCheckPos.localPosition.x), startCheckPos.localPosition.y, startCheckPos.localPosition.z);
                    }
                    else
                    {
                        //在左边
                        speed = -Mathf.Abs(speed);
                        startCheckPos.localPosition = new Vector3(-Mathf.Abs(startCheckPos.localPosition.x), startCheckPos.localPosition.y, startCheckPos.localPosition.z);
                    }
                    SetSpeedX(speed);
                }
                else
                {
                    SetSpeedX(0);
                    enemyStatus = EnemyStatus.Idle;
                }
                break;
            case EnemyStatus.Dead:

                animator.SetBool("isDead", true);

                break;
        }

        if (enemyStatus != EnemyStatus.Walk)
        {
            animator.SetBool("isWalk", false);
        }
        if (enemyStatus != EnemyStatus.Run)
        {
            animator.SetBool("isRun", false);
        }
        if (enemyStatus != EnemyStatus.Attack)
        {
            animator.SetBool("isAttack", false);
        }
    }

    

    public override void OnDead(string resetPos)
    {
        SetSpeedX(0);
        base.OnDead(resetPos);
    }

    #endregion
}
