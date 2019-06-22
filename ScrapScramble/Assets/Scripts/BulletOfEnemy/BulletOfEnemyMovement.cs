using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOfEnemyMovement : MonoBehaviour
{
    //弾が進むスピード
    public float speed = 2.0f;

    //弾の威力（プレイヤーの資源を落とさせる量）
    int power = 1;

    //壁にぶつかっても消滅しなかったとき、一定距離進むことで消滅させる
    const float disappearDistance = 1500.0f;

    //進んだ距離
    float advanceDistance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //前に進む
        gameObject.transform.position += gameObject.transform.forward * speed;

        //距離の記録
        advanceDistance += speed;

        //一定距離進んだとき消滅
        if (advanceDistance >= disappearDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーに当たった時
        if (other.tag == "Player")
        {
            {
                PlayerMovememt move = other.GetComponent<PlayerMovememt>();
                if (!move)
                {
                    Debug.Log("エネミー弾：プレイヤーの資源のドロップに失敗");
                }

                move.DropResource((uint)power);
            }

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

    public void SetPower(int value)
    {
        power = value;
    }
}
