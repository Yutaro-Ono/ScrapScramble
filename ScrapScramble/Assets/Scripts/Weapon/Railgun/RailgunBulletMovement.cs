using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBulletMovement : MonoBehaviour
{
    // ウェーブの情報
    WaveManagement waveManager;

    // 発射したプレイヤーのオブジェクト
    GameObject shooterPlayer;

    // ヒットしたプレーヤーの情報を保存する
    GameObject prevPlayerObj;

    // ヒットしたエネミーの情報のリスト
    List<GameObject> hitEnemies = new List<GameObject>(2);

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

    // レイヤーの設定を行ったかどうか
    bool layerSettingFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        numHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // レイヤーの設定
        if (!layerSettingFlag)
        {
            bool isVsPlayerWave = (waveManager.wave == WaveManagement.WAVE_NUM.WAVE_2_PVP || waveManager.wave == WaveManagement.WAVE_NUM.WAVE_4_PVP);
            gameObject.layer = LayerMask.NameToLayer(isVsPlayerWave ? WeaponEnumDefine.TouchablePlayerLayerName : WeaponEnumDefine.UntouchablePlayerLayerName);
            layerSettingFlag = true;
        }

        //前に進む
        gameObject.transform.position += gameObject.transform.forward * speed;

        //距離の記録
        advanceDistance += speed;

        //一定距離進んだとき消滅
        if (advanceDistance >= disappearDistance)
        {
            Destroy(gameObject);
        }

        // インターバルウェーブで消滅
        if (waveManager.wave == WaveManagement.WAVE_NUM.WAVE_INTERVAL)
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
                    PlayerMovement move = other.GetComponent<PlayerMovement>();
                    if (!move)
                    {
                        Debug.Log("レールガン：プレイヤーの資源のドロップに失敗");
                    }

                    move.DropResource((uint)power);
                }
                PlayerStatus status = other.GetComponent<PlayerStatus>();
                Debug.Log("レールガンの弾がプレイヤーにヒット");

                // まだどのプレイヤーにもあたっていないとき
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

                // 当たったプレーヤーの情報を保存
                prevPlayerObj = other.gameObject;

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

            // これまで当たってきたものの中に、今のオブジェクトがなければ
            if (!hitEnemies.Contains(other.gameObject))
            {
                // ヒットカウントを進める
                numHit++;

                // リストに追加
                hitEnemies.Add(other.gameObject);
            }

            if (numHit >= maxHit)
            {
                Destroy(gameObject);
                numHit = 0;
            }
        }

        //壁に当たった時
        else if (other.tag == "Wall")
        {
            Debug.Log("レールガンの弾が壁にぶち当たった");

            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void SetShooterPlayer(GameObject in_shooterPlayer)
    {
        this.shooterPlayer = in_shooterPlayer;
        waveManager = shooterPlayer.GetComponent<PlayerStatus>().GetWaveManager();
    }
}
