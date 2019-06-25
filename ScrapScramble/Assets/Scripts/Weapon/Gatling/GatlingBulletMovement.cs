using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingBulletMovement : MonoBehaviour
{
    //発射したプレイヤーのオブジェクト
    GameObject shooterPlayer;

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
            //そのプレイヤ－が装備者自身でないとき
            if (other.gameObject != shooterPlayer)
            {
                {
                    PlayerMovememt move = other.GetComponent<PlayerMovememt>();
                    if (!move)
                    {
                        Debug.Log("ガトリング弾：プレイヤーの資源のドロップに失敗");
                    }

                    move.DropResource((uint)power);
                }
                PlayerStatus status = other.GetComponent<PlayerStatus>();
                Debug.Log("ガトリング：プレイヤー" + (status.GetId() + 1) + "にヒット");

                Destroy(gameObject);
            }
        }

        // エネミーに当たった時
        else if (other.tag == "Enemy")
        {
            EnemyStatus status = other.GetComponent<EnemyStatus>();

            status.hitPoint -= (short)power;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //壁に当たった時
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("ガトリングの弾が壁にぶち当たった");

            Destroy(gameObject);
        }
    }

    public void SetPower(int value)
    {
        power = value;
    }

    public void SetShooterPlayer(GameObject in_shooterPlayer)
    {
        this.shooterPlayer = in_shooterPlayer;
    }
}
