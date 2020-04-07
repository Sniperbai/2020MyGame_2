using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region 字段

    Rigidbody2D rigidbody2d;
    SpriteRenderer spriteRenderer;

    public float speed;

    Damage damage;

    Animator animator;

    #endregion

    #region  Unity回调

    public void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        damage = transform.GetComponent<Damage>();

        //SetDirection(false);

        animator = transform.GetComponent<Animator>();
    }

    public void SetDirection(bool isRight)
    {
        spriteRenderer.flipX = !isRight;
        rigidbody2d.velocity = new Vector2(isRight?speed:-speed,0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    
        //对游戏物体造成伤害
        damage.OnDamage(collision.gameObject);

        animator.SetBool("isBomb",true);

        rigidbody2d.velocity = Vector2.zero;
        transform.GetComponent<BoxCollider2D>().enabled = false;

        //销毁自己
        Destroy(gameObject,0.15f);

    }

    #endregion



}
