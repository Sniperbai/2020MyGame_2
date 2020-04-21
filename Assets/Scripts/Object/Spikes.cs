using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    Damage damage;

    GameObject attackObject;

    void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
        damage = transform.GetComponent<Damage>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //damage.OnDamage(collision.gameObject);
        attackObject = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        attackObject = null;
    }

    private void Update()
    {
        if (attackObject != null)
        {
            damage.OnDamage(attackObject);
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    damage.OnDamage(collision.gameObject);
    //}
}
