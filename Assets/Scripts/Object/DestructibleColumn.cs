using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleColumn : MonoBehaviour
{
    Damageable damageable;
    GameObject destoryObj;

    private void Start()
    {
        destoryObj = transform.Find("Destructible").gameObject;

        damageable = transform.GetComponent<Damageable>();
        damageable.OnDead += this.OnDead;
    }

    public void OnDead(string ResetPos)
    {
        destoryObj.SetActive(true);
        transform.GetComponent<BoxCollider2D>().enabled = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject,3);
    }
}
