using System.Collections;
using System.Collections.Generic;
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

    Rigidbody2D rigidbody2D;

    public float speed;

    Transform startCheckPos;     //检测前方是否可以移动

    public bool isCanMove = true;

    public EnemyStatus enemyStatus;

    float idleTimer;

    SpriteRenderer spriteRenderer;

    Animator animator;

    #endregion

    #region Unity回调

    private void Start()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();

        startCheckPos = transform.Find("startCheckPos");

        enemyStatus = EnemyStatus.Idle;

        spriteRenderer = transform.GetComponent<SpriteRenderer>();

        animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        //SetSpeedX(speed);

        CheckIsCanMove();     //检测是否可以移动

        UpdateStatus();
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
                //
               
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

                

                break;
            
            case EnemyStatus.Run:
                break;
            case EnemyStatus.Dead:
                break;
        }

        if (enemyStatus != EnemyStatus.Walk)
        {
            animator.SetBool("isWalk", false);
        }
    }

    public void SetSpeedX(float speedX)
    {
        spriteRenderer.flipX = speedX < 0;
        rigidbody2D.velocity = new Vector2(speedX, rigidbody2D.velocity.y);
    }

    #endregion
}
