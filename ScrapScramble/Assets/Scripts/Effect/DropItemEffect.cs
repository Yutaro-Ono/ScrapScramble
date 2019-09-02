using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemEffect : MonoBehaviour
{
    public GameObject enemy;
    public GameObject drop;
    public GameObject dropEffect;
    Transform transfor;
    GameObject ins;
    EnemyDrop enemydrop;

    // Start is called before the first frame update
    void Start()
    {
        transfor = drop.GetComponent<Transform>();
        enemydrop = enemy.GetComponent<EnemyDrop>();
    }

    // Update is called once per frame
    void Update()
    {
        DropEffect();
    }
    void DropEffect()
    {
        if(enemydrop.GetDroppedFlag()==true)
        {
            ins = Instantiate(dropEffect, transfor.position, transfor.rotation) as GameObject;
           
        }
        if (enemydrop.GetDroppedFlag() == false)
        {
            Destroy(ins, 0.5f);
        }


    }
}
