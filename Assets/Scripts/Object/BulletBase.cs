using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    #region 字段

    protected Rigidbody2D rigidbody2d;
    protected SpriteRenderer spriteRenderer;

    protected Damage damage;

    protected Animator animator;

    #endregion

    #region  Unity回调

    public virtual void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        damage = transform.GetComponent<Damage>();

        //SetDirection(false);

        animator = transform.GetComponent<Animator>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        //对游戏物体造成伤害
        damage.OnDamage(collision.gameObject);

        animator.SetBool("isBomb", true);

        rigidbody2d.velocity = Vector2.zero;
        transform.GetComponent<Collider2D>().enabled = false;

        //销毁自己
        Destroy(gameObject, 0.15f);

    }

    #endregion
}
