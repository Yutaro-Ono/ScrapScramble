using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public GameObject enemyDamageEffect;
    Transform transfor;
    GameObject ins;
    EnemyStatus status;

    void Start()
    {
        transfor = GetComponent<Transform>();
        status = GetComponent<EnemyStatus>();

    }

    void Update()
    {
        Damage();
    }
    void Damage()
    {
        if (status.hitPoint< 0)
        {
            ins = Instantiate(enemyDamageEffect, transfor) as GameObject;
            Destroy(ins, 1.52f);
        }
       
    }
}
