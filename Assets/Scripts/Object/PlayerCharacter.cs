using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus {

    Idle = 0,
    Jump = 0,
    Run = 2,
    Crouch = 3,

}


public class PlayerCharacter : MonoBehaviour
{
    #region 字段

    Rigidbody2D rigidbody2d;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;     // 渲染人物

    public float speedX;               //X方向的速度
    public float speedY;               //Y方向的速度

    Animator animator;                 //动画控制器

    float timerY;                      //上跳的计时器

    public bool isGround;               //是不是在地面上

    public bool jump;                  //跳跃的状态

    public PlayerStatus currentStatus = PlayerStatus.Idle;   //当前的状态

    Transform followTarget;             //相机跟随的目标
    public Vector3 followTargetOffset;  //跟随的偏移

    PassPlatform currentPlatform;      //当前处在的平台

    public Transform startCheckPos;     //开始检查是否在地面的开始点
    public ContactFilter2D ContactFilter;
    RaycastHit2D[] m_HitBuffer = new RaycastHit2D[5];
    public int count;

    Damageable playerDamageable;

    #endregion

    #region Unity回调

    private void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        capsuleCollider = transform.GetComponent<CapsuleCollider2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        followTarget = transform.Find("followTarget");
        followTarget.position = transform.position + followTargetOffset;
        startCheckPos = transform.Find("startCheckPos");

        playerDamageable = transform.GetComponent<Damageable>();  //初始化受伤功能
        playerDamageable.OnHurt += this.OnHurt;                   //注册受伤事件
        playerDamageable.OnDead += this.OnDead;                   //注册死亡事件

    }

    private void Update()
    {

        UpdateVelocity();    //更新速度

        CheckGround();       //检测是否在地面

        UpdateStatus();      //更新状态

        UpdateAnimator();    //更新动画

        UpdateFollowTargetPos();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentPlatform = collision.gameObject.GetComponent<PassPlatform>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        currentPlatform = null;
    }

    #endregion


    #region 方法

    //设置X方向的速度
    public void SetSpeedX(float x) {

        animator.SetBool("isRun" ,x != 0);


        if (x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if ( x > 0 )
        {
            spriteRenderer.flipX = false;
        }

        if (currentStatus == PlayerStatus.Crouch) x = 0;

        rigidbody2d.velocity = new Vector2(x,rigidbody2d.velocity.y);
    }

    //设置Y方向的速度
    public void SetSpeedY(float y) {

        if (currentStatus == PlayerStatus.Crouch) y = 0;

        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x,y);
    }

    //检测是否在地面
    public void CheckGround() {

        RaycastHit2D raycastHit2D = Physics2D.Raycast(startCheckPos.position, Vector3.down, 0.2f, 1 << 8);
        isGround = raycastHit2D;

        if (raycastHit2D)
        {
            if (raycastHit2D.collider.tag == TagConst.SkyGround)
            {
                if (raycastHit2D.point.y < raycastHit2D.transform.position.y + raycastHit2D.transform.GetComponent<BoxCollider2D>().offset.y)
                {
                    isGround = false;
                }
                else
                {
                    isGround = true;
                }

             
            }
            else
            {
                isGround = true;
            }
        }
        else
        {
            isGround = false;
        }
        Debug.DrawLine(startCheckPos.position, startCheckPos.position + Vector3.down * 0.2f, Color.red);

        //if (raycastHit2D)
        //{
        //    Debug.Log("检测到游戏物体：" + raycastHit2D.collider.gameObject.name);

        //}
    }

    //更新玩家状态
    public void UpdateStatus()
    {
        currentStatus = PlayerStatus.Idle;
        if (rigidbody2d.velocity.x != 0)
        {
            currentStatus = PlayerStatus.Run;
        }

        if (!isGround)
        {
            currentStatus = PlayerStatus.Jump;
        }

        if (PlayerInput.instance.Vertical.value == -1 && isGround)
        {
            currentStatus = PlayerStatus.Crouch;
        }

        //判断下落
        if (PlayerInput.instance.Vertical.value == -1 && isGround && PlayerInput.instance.Jump.Down)
        {
            if (currentPlatform != null)
            {
                currentPlatform.Fall(gameObject);
                animator.SetTrigger("fall");
            }
        }


    }

    //更新动画
    public void UpdateAnimator()
    {
        animator.SetBool("isJump", !isGround);
        animator.SetFloat("speedY",this .rigidbody2d.velocity.y);
        animator.SetBool("isCrouch",PlayerInput.instance.Vertical.value == -1);
    }

    //更新跳跃
    public bool UpdateJump()
    {
        if (PlayerInput.instance.Jump.Down && isGround)
        {
            timerY = 0;
            jump = true;
        }

        if (PlayerInput.instance.Jump.Hold && jump)
        {
            timerY += Time.deltaTime;

            if (timerY < 0.2)
            {

                jump = true;
            }
            else
            {

                jump = false;
            }

        }

        if (PlayerInput.instance.Jump.Up)
        {
            jump = false;

        }

        return jump;
        
    }

    //更新速度
    public void UpdateVelocity()
    {
        //更新x方向速度
        SetSpeedX(PlayerInput.instance.Horizontal.value * speedX);

        //更新y方向速度
        if (UpdateJump()) SetSpeedY(speedY); 
    }

    //更新跟随位置
    public void UpdateFollowTargetPos()
    {
        if (spriteRenderer.flipX)
        {
            followTarget.position = Vector3.MoveTowards(followTarget.position, transform.position - followTargetOffset, 0.1f);
        }
        else
        {
            followTarget.position = Vector3.MoveTowards(followTarget.position, transform.position + followTargetOffset, 0.1f);
        }

    }

    #endregion

    #region 受伤相关的方法

    public void OnHurt()
    {
        //播放受伤动画
        animator.SetTrigger("hurt");

        //更新血条
    }

    public void OnDead()
    {
        //播放死亡动画

        //返回菜单
    }

    public void Attack()
    {

    }

    #endregion
}


