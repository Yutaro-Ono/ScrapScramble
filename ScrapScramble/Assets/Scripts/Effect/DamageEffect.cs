using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public GameObject damage;
    Transform transfor;
    GameObject ins;
    // Start is called before the first frame update
    void Start()
    {
        transfor = GetComponent<Transform>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BulletOfEnemy")
        {
            ins=Instantiate(damage, transfor.position, transfor.rotation)as GameObject;
            Destroy(ins, 0.5f);
        }
        if (collision.gameObject.tag == "BulletOfPlayer")
        {
            ins = Instantiate(damage, transfor.position, transfor.rotation) as GameObject;
            Destroy(ins, 0.5f);
        }
    }
}
