using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOfEnemyMovement : MonoBehaviour
{
    //弾が進むスピード
    public float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //前に進む
        gameObject.transform.position += gameObject.transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーに当たった時
        if (other.tag == "Player")
        {
            Debug.Log("エネミーの弾がプレイヤーにヒット");

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //壁に当たった時
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("エネミーの弾が壁にぶち当たった");

            Destroy(gameObject);
        }
    }
}
