using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class Damage : MonoBehaviour
{
    public int damage;     //伤害

    //对别人造成伤害
    public void OnDamage(GameObject gameObject)
    {
        //TODO
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if (damageable == null)
        {
            return;
        }
        damageable.TakeDamage(this.damage);
    }

    public void OnDamage(GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            OnDamage(gameObjects[i]);
        }

    }

}


