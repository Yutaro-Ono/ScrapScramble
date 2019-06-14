using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOfEnemyMovement : MonoBehaviour
{
    public static ushort playerNum = 4;

    GameObject[] playerObj = new GameObject[playerNum];
    Quaternion[] playerRota = new Quaternion[playerNum];

    float[] playerRotaX = new float[playerNum];
    float[] playerRotaY = new float[playerNum];
    float[] playerRotaZ = new float[playerNum];

    //弾が進むスピード
    public float speed = 2.0f;

    //弾の威力（プレイヤーの資源を落とさせる量）
    public int power = 1;

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

    private void FixedUpdate()
    {
        //プレイヤーの回転情報を記録
        for (int i = 0; i < 4; i++)
        {
            playerRota[i] = playerObj[i].transform.rotation;

            playerRotaX[i] = playerRota[i].x;
            playerRotaY[i] = playerRota[i].y;
            playerRotaZ[i] = playerRota[i].z;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーに当たった時
        if (other.tag == "Player")
        {
            //接触によるプレイヤーの回転を元に戻す
            //プレイヤーを走査して、該当するオブジェクトの回転情報を代入
            for (int i = 0; i < playerNum; i++)
            {
                if (other.gameObject == playerObj[i])
                {
                    other.gameObject.transform.rotation = Quaternion.Euler(new Vector3(playerRotaX[i], playerRotaY[i], playerRotaZ[i]));
                    break;
                }
            }

            {
                //ここでプレイヤーが資源をドロップする関数を呼ぶ
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

    public void SetPlayerObject(GameObject[] in_player, int playerNum)
    {
        for (int i = 0; i < playerNum; i++)
        {
            playerObj[i] = in_player[i];
        }
    }
}
