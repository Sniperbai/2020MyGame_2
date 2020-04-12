using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyStatus
{
    Idle,
    Walk,
    Run,
    Dead,
    Attack,

}

//敌人
public class Chomper : MonoBehaviour
{
    #region 字段

    Rigidbody2D rigidbody2d;

    public float speed;

    Transform startCheckPos;     //检测前方是否可以移动

    public bool isCanMove = true;

    public EnemyStatus enemyStatus;

    float idleTimer;

    SpriteRenderer spriteRenderer;

    Animator animator;

    Damage damage;
    Damageable damageable;

    public float attackRange;     //攻击的范围
    public float listenRange;     //监听的范围

    public Transform attackTarget;  //攻击的目标

    #endregion

    #region Unity回调

    private void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();

        startCheckPos = transform.Find("startCheckPos");

        enemyStatus = EnemyStatus.Idle;

        spriteRenderer = transform.GetComponent<SpriteRenderer>();

        animator = transform.GetComponent<Animator>();

        damage = transform.GetComponent<Damage>();

        damageable = transform.GetComponent<Damageable>();
        damageable.OnDead += OnDead;

        attackTarget = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        //SetSpeedX(speed);

        CheckIsCanMove();     //检测是否可以移动

        UpdateStatus();

        UpdateListener();     //监听敌人
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(Color.red.r,Color.red.g,Color.red.b,0.2f);   
        Handles.DrawSolidDisc(transform.position, Vector3.forward, attackRange);

        Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.forward, listenRange);
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

        Debug.DrawLine(startCheckPos.position, startCheckPos.position + Vector3.down,Color.red);

        isCanMove = raycastHit2D;
    }

    public void UpdateStatus()
    {
        switch (enemyStatus)
        {
            case EnemyStatus.Idle:

                SetSpeedX(0);
                idleTimer += Time.deltaTime;
                if (idleTimer > 2)
                {
                    idleTimer = 0 ;
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
                animator.SetBool("isWalk",true);

                break;

            case EnemyStatus.Attack:

                //设置奔跑动画
                animator.SetBool("isAttack", true);

                break;
            
            case EnemyStatus.Run:

                //设置奔跑动画
                animator.SetBool("isRun",true);
                //跑向要攻击的目标
                if (isCanMove)
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
                    enemyStatus = EnemyStatus.Idle;
                }
                break;
            case EnemyStatus.Dead:

                animator.SetBool("isDead",true);

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

    public void SetSpeedX(float speedX)
    {
        spriteRenderer.flipX = speedX < 0;
        rigidbody2d.velocity = new Vector2(speedX, rigidbody2d.velocity.y);
    }

    //监听要攻击的目标
    public void UpdateListener()
    {
        if (attackTarget == null)
        {
            Debug.LogWarning("要攻击的目标为空！");
            return;
        }

        if (Vector3.Distance(transform.position, attackTarget.position) <= attackRange)
        {
            //可以进行攻击了
            Debug.Log("可以进行攻击了！");
            enemyStatus = EnemyStatus.Attack;
            return;
        }
        //else
        //{
        //    enemyStatus = EnemyStatus.Idle;
        //}

        if (Vector3.Distance(transform.position, attackTarget.position) <= listenRange)
        {
            //发现了敌人
            //播放run动画
            enemyStatus = EnemyStatus.Run;
        }
        //else
        //{
        //    enemyStatus = EnemyStatus.Idle;
        //}

       
    }

    public void OnAttack()
    {
        damage.OnDamage(attackTarget.gameObject);
    }

    public void OnDead(string resetPos)
    {
        SetSpeedX(0);
        enemyStatus = EnemyStatus.Dead;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rigidbody2d.gravityScale = 0;
        rigidbody2d.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject,0.5f);
    }

    #endregion
}
