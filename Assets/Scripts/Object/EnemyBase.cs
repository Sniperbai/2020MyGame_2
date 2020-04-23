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

public class EnemyBase : MonoBehaviour
{
    #region 字段

    protected Rigidbody2D rigidbody2d;

    public EnemyStatus enemyStatus;

    protected SpriteRenderer spriteRenderer;

    protected Animator animator;

    protected Damage damage;
    protected Damageable damageable;

    public float attackRange;     //攻击的范围
    public float listenRange;     //监听的范围

    public Transform attackTarget;  //攻击的目标

    #endregion

    #region Unity的回调

    protected virtual void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();

        enemyStatus = EnemyStatus.Idle;

        spriteRenderer = transform.GetComponent<SpriteRenderer>();

        animator = transform.GetComponent<Animator>();

        damage = transform.GetComponent<Damage>();

        damageable = transform.GetComponent<Damageable>();
        damageable.OnDead += OnDead;

        attackTarget = GameObject.Find("Player").transform;
    }

    protected virtual void Update()
    {
        OnUpdateStatus();

        UpdateListener();     //监听敌人
 
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.forward, attackRange);

        Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.forward, listenRange);
    }

    #endregion

    #region 方法

    //更新状态
    public virtual void OnUpdateStatus()
    {
        
    }

    //监听敌人
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

        if (Vector3.Distance(transform.position, attackTarget.position) <= listenRange)
        {
            //发现了敌人
            //播放run动画
            enemyStatus = EnemyStatus.Run;
        }

    }

    public void SetSpeedX(float speedX)
    {
        if (speedX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (speedX < 0)
        {
            spriteRenderer.flipX = true;
        }

        rigidbody2d.velocity = new Vector2(speedX, rigidbody2d.velocity.y);
    }

    public virtual void OnAttack()
    {
        damage.OnDamage(attackTarget.gameObject);
    }

    public virtual void OnDead(string resetPos)
    {
        enemyStatus = EnemyStatus.Dead;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rigidbody2d.gravityScale = 0;
        rigidbody2d.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 0.5f);
    }


    #endregion

}
