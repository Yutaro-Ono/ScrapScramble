using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public GameObject damagePlayer;
    public GameObject damage;
    public Transform transfor;
    GameObject expl;
    // Start is called before the first frame update
    void Start()
    {
        transfor = damagePlayer.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collider other)
    {
        if (other.gameObject.name == "Enemy's Bullet")
        {
            expl=Instantiate(damage, transfor.position, transfor.rotation)as GameObject;
            Destroy(expl, 1.1f);
        }
    }
}
