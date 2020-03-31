using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum HurtType
{
    Normal,
    Dead
}

public class Damage : MonoBehaviour
{
    public int damage;     //伤害
    public HurtType hurtType;
    public string ResetPos;

    //对别人造成伤害
    public void OnDamage(GameObject gameObject)
    {
        //TODO
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if (damageable == null)
        {
            return;
        }
        damageable.TakeDamage(this.damage, hurtType, ResetPos);
    }

    public void OnDamage(GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            OnDamage(gameObjects[i]);
        }

    }

}


