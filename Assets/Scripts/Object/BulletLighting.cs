using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLighting : MonoBehaviour
{
    Damage damage;

    private void Start()
    {
        damage = transform.GetComponent<Damage>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Invoke("Hide", 2);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagConst.Player)
        {
            damage.OnDamage(collision.gameObject);
        }
    }

}
