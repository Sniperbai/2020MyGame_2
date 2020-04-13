﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus {

    Idle = 0,
    Jump = 0,
    Run = 2,
    Crouch = 3,

}

public enum AttackType
{
    Attack = 0,    //普通攻击
    Shoot = 1       //射击
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

    Damageable playerDamageable;        //可受伤的
    Damage playerDamage;               //对别的游戏物体造成伤害

    string ResetPos;         //玩家受伤出生的点

    float attackTime = 0.4f;        //攻击的冷却时间
    bool attackIsReady = true;      //是否冷却好了

    float setSpeedXTime;            //设置 x 方向速度的时间
    float setSpeedXTimer;            //设置 x 方向速度的计时器
    float setSpeedX;                //速度的大小

    AttackRange attackRange;          //人物攻击的范围
    Transform bulletSpawnPos;         //子弹生成的位置
    GameObject bulletPrefab;          //子弹的预制体

    public bool defaultHaveWeapon;    //默认是否有武器

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

        playerDamage = transform.GetComponent<Damage>();
        playerDamageable = transform.GetComponent<Damageable>();  //初始化受伤功能
        playerDamageable.OnHurt += this.OnHurt;                   //注册受伤事件
        playerDamageable.OnDead += this.OnDead;                   //注册死亡事件

        //只更新一次
        if (!DataManager.Instance.IsContainsData(DataConst.hp))
        {
            //保存最大的血量值
            SaveMaxHp(playerDamageable.health);
            UpdateHpData(playerDamageable.health);
        }
        else
        {
            playerDamageable.health = GetHpFromData();   //设置血量
        }
        
        GamePanel.Instance.InitHP(GetMaxHp(), GetHpFromData());
        attackRange = transform.Find("attackRange").GetComponent<AttackRange>();

        bulletSpawnPos = transform.Find("bulletSpawnPos");

        //判断默认是不是有武器(初始化武器)
        if (defaultHaveWeapon)
        {
            //对数据进行保存
            Data<bool> data = new Data<bool>();
            data.value1 = true;
            DataManager.Instance.SaveData(DataConst.is_have_weapon, data);
        }

    }

    private void Update()
    {

        UpdateVelocity();    //更新速度

        UpdateSetSpeedXWithTime();  

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
            attackRange.transform.localPosition = new Vector3(-1.115f,attackRange.transform.localPosition.y,0);

        }
        else if ( x > 0 )
        {
            spriteRenderer.flipX = false;
            attackRange.transform.localPosition = new Vector3(1.115f, attackRange.transform.localPosition.y, 0);

        }

        if (currentStatus == PlayerStatus.Crouch) x = 0;

        rigidbody2d.velocity = new Vector2(x,rigidbody2d.velocity.y);
    }

    public void UpdateSetSpeedXWithTime()
    {
        setSpeedXTimer += Time.deltaTime;
        if (setSpeedXTimer < setSpeedXTime)
        {
            rigidbody2d.velocity = new Vector2(setSpeedX, rigidbody2d.velocity.y);
        }
    }

    public void SetSpeedXWithTime(float x, float time)
    {
        setSpeedXTime = time;
        setSpeedXTimer = 0;
        setSpeedX = x;
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

        //普通攻击
        if (PlayerInput.instance.Attack.Down || PlayerInput.instance.Attack.Hold)
        {
            Attack(AttackType.Attack);
        }

        //射击
        if (PlayerInput.instance.Shoot.Down || PlayerInput.instance.Shoot.Hold)
        {
            Attack(AttackType.Shoot);
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

    public void Attack(AttackType attackType)
    {
        if (!IsHaveWeapon()) return;         //没有武器不能攻击

        if (!attackIsReady) { return; }      //技能还没有冷却完成

        Debug.Log("攻击:" + attackType);

        animator.SetTrigger("attack");
        animator.SetTrigger("trigger");
        animator.SetInteger("attackType",(int)attackType);

        if (attackType == AttackType.Attack)
        {
            SetSpeedXWithTime(spriteRenderer.flipX ? -7 : 7, 0.2f);
        }
        else
        {
            animator.SetFloat("shoot",1);
            //创建一个子弹
            Invoke("CreateBullet",0.02f);
        }

        attackIsReady = false;
        Invoke("ResetAttackIsReady", attackTime);
    }

    //创建一个子弹
    void CreateBullet()
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("Prefabs/Object/Bullet");
        }
        GameObject gameObject = GameObject.Instantiate(bulletPrefab);

        if (spriteRenderer.flipX)
        {
            bulletSpawnPos.localPosition = new Vector3(-bulletSpawnPos.localPosition.x, bulletSpawnPos.localPosition.y, bulletSpawnPos.localPosition.z);
        }

        gameObject.transform.position = bulletSpawnPos.position;
        gameObject.GetComponent<Bullet>().SetDirection(!spriteRenderer.flipX);
    }

    public void ResetAttackIsReady()
    {
        attackIsReady = true;
        animator.SetFloat("shoot",0);
    }

    //是否有武器
    public bool IsHaveWeapon()
    {
        Data data = DataManager.Instance.GetData(DataConst.is_have_weapon);
        if(data != null && ((Data<bool>)data).value1)
        {
            return true;
        }
        
        return false;
    }

    //更新血量数据
    public void UpdateHpData(int hp)
    {
        Data data = DataManager.Instance.GetData(DataConst.hp);
        if (data == null)
        {
            data = new Data<int>();
            ((Data<int>)data).value1 = hp;
            DataManager.Instance.SaveData(DataConst.hp, data);
        }
        else
        {
            ((Data<int>)data).value1 = hp;
        }
    }

    //获取血量
    public int GetHpFromData()
    {
        Data<int> data = (Data<int>)DataManager.Instance.GetData(DataConst.hp);
        return data.value1;
    }

    //保存血量上限
    public void SaveMaxHp(int maxHp)
    {
        Data<int> data = new Data<int>();
        data.value1 = maxHp;
        DataManager.Instance.SaveData(DataConst.maxHp,data);
    }

    //获取血量上限
    public int GetMaxHp()
    {
        Data<int> data =(Data<int>) DataManager.Instance.GetData(DataConst.maxHp);
        return data.value1;
    }

    #endregion

    #region 受伤相关的方法

    //设置无敌状态
    public void SetWudi(int time)
    {
        animator.SetBool("isWudi", true);
        playerDamageable.Disable();
        Invoke("ResetDamageable", time);

        //不会跟敌人层发生碰撞 
        Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("player"), ~LayerMask.GetMask("enemy", "IgnorePlayer"));

    }

    //设置死亡的状态
    public void SetDead()
    {
        //播放死亡动画
        animator.SetBool("isDead", true);
        animator.SetTrigger("trigger");
        rigidbody2d.gravityScale = 0;
        rigidbody2d.velocity = Vector2.zero;

        //取消控制
        PlayerInput.instance.SetEnable(false);

        // 显示提示
        TipMessagePanel.Instance.ShowTip(null, TipStyle.Style2);


    }

    public void OnHurt(HurtType hurtType ,string ResetPos)
    {
        //更新内存数据
        UpdateHpData(playerDamageable.health);

        this.ResetPos = ResetPos; 

        switch (hurtType)
        {
            case HurtType.Normal:
                //播放受伤动画
                animator.SetTrigger("hurt");

                //设置无敌状态
                SetWudi(1);

                break;
            case HurtType.Dead:
                //播放死亡动画
                SetDead();

                //重置玩家位置
                Invoke("ResetDead",1);
                break;
           
        }

        

        //更新血条
        GamePanel.Instance.UpdateHP(GetHpFromData());

        
    }

    public void ResetDamageable()
    {
        playerDamageable.Enanble();
        animator.SetBool("isWudi", false);
        //可以跟敌人层发生碰撞 
        Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("player"), ~LayerMask.GetMask("IgnorePlayer"));
    }

    public void ResetDead()
    {
        animator.SetBool("isDead", false);
        rigidbody2d.gravityScale = 5;
        PlayerInput.instance.SetEnable(true);

        //给他一段时间的无敌状态
        SetWudi(1);

        //设置位置
        transform.position = GameObject.Find(ResetPos).transform.position;
    }

    public void OnDead(string resetPos)
    {
        ResetPos = resetPos;
        //Debug.Log("游戏结束");

        //设置成死亡状态
        SetDead();

        Invoke("DelayShowGameOverPanel",1);
        //更新血条
        GamePanel.Instance.UpdateHP(GetHpFromData());

    }

    public void DelayShowGameOverPanel()
    {
        //显示游戏结束的界面
        TipMessagePanel.Instance.ShowTip(null, TipStyle.Style3);
        //ResetPos = "Spawn1";
        //重置死亡状态
        ResetDead();

        //重置 Hp
        GamePanel.Instance.ResetHP();
        playerDamageable.ResetHealth();
    }

    //造成伤害
    public void AttackDamage()
    {
        //获取到所有攻击的游戏物体
        GameObject[] damageables = attackRange.GetDamageableGameObjects();
        if (damageables != null && damageables.Length != 0)
        {
            //对这些游戏物体造成伤害
            playerDamage.OnDamage(damageables);
        }

    }

    #endregion
}


