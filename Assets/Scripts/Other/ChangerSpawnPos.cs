using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerSpawnPos : MonoBehaviour
{
    public Damage[] needChangeDamages;

    public string TargetSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConst.Player)
        {
            for (int i = 0; i < needChangeDamages.Length; i++)
            {
                needChangeDamages[i].ResetPos = TargetSpawn;
            }
           
        }
    }
}
