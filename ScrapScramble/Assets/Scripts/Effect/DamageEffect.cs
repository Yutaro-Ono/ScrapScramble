using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public GameObject damage;
    public Transform transfor;
    // Start is called before the first frame update
    void Start()
    {
        transfor = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Enemy's Bullet")
        {
            Instantiate(damage, transfor.position, transfor.rotation);
        }
    }
}
