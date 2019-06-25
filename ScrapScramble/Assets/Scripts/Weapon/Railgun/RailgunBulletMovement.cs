using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBulletMovement : MonoBehaviour
{
    // 発射したプレイヤーのオブジェクト
    GameObject shooterPlayer;

    // ヒットしたプレーヤーの情報を保存する
    GameObject prevPlayerObj;

    //弾が進むスピード
    public float speed = 3.0f;

    //弾の威力（プレイヤーの資源を落とさせる量）
    public int power = 100;

    //壁にぶつかっても消滅しなかったとき、一定距離進むことで消滅させる
    const float disappearDistance = 1500.0f;

    //進んだ距離
    float advanceDistance = 0.0f;

    // 最大ヒット数
    const int maxHit = 3;

    // ヒットカウント
    int numHit;

    // Start is called before the first frame update
    void Start()
    {
        numHit = 0;
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
            //そのプレイヤ－が装備者自身でないとき
            if (other.gameObject != shooterPlayer)
            {
                {
                    PlayerMovememt move = other.GetComponent<PlayerMovememt>();
                    if (!move)
                    {
                        Debug.Log("レールガン：プレイヤーの資源のドロップに失敗");
                    }

                    move.DropResource((uint)power);
                }
                PlayerStatus status = other.GetComponent<PlayerStatus>();
                Debug.Log("レールガンの弾がプレイヤーにヒット");

                // 当たったプレーヤーの情報を保存
                prevPlayerObj = other.gameObject;

                if(numHit == 0)
                {
                    numHit++;
                }

                // 当たったのが同じプレーヤーでなければヒットカウントを進める
                else if(prevPlayerObj != other.gameObject)
                {
                    // ヒットカウントを進める
                    numHit++;
                }

                if (numHit >= maxHit)
                {
                    Destroy(gameObject);
                    numHit = 0;
                }
            }
        }
        // エネミーに当たった時
        else if (other.tag == "Enemy")
        {
            EnemyStatus status = other.GetComponent<EnemyStatus>();
            Debug.Log("レールガンの弾がエネミーにヒット");
            status.hitPoint -= (short)power;

            // ヒットカウントを進める
            numHit++;

            if (numHit >= maxHit)
            {
                Destroy(gameObject);
                numHit = 0;
            }
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
