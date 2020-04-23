using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public List<GameObject> damageables = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != TagConst.Player)
        {
            Damageable damageable = collision.transform.GetComponent<Damageable>();

            if (damageable != null)
            {
                damageables.Add(damageable.gameObject);
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != TagConst.Player)
        {
            Damageable damageable = collision.transform.GetComponent<Damageable>();

            if (damageable != null)
            {
                if (!damageables.Contains(damageable.gameObject))
                {
                    damageables.Add(damageable.gameObject);
                }

            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Damageable damageable = collision.transform.GetComponent<Damageable>();

        if (damageable != null)
        {
            damageables.Remove(damageable.gameObject);
        }
    }

    public GameObject[] GetDamageableGameObjects()
    {
        return damageables.ToArray();
    }
}
