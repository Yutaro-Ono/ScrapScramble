using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBulletMovement : MonoBehaviour
{
    //弾が進むスピード
    public float speed = 3.0f;

    //弾の威力（プレイヤーの資源を落とさせる量）
    public int power = 3;

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
                //ここでプレイヤーが資源をドロップする関数を呼ぶ
            }

            Debug.Log("レールガンの弾がプレイヤーにヒット");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //壁に当たった時
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("レールガンの弾が壁にぶち当たった");

            Destroy(gameObject);
        }
    }
}
