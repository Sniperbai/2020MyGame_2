using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Damage damage;

    private void Start()
    {
        damage = transform.GetComponent<Damage>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damage.OnDamage(collision.gameObject);
    }
}

