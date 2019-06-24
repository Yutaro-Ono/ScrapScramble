using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    EnemyStatus enemyStatus;

    //資源・武装をドロップしたかのフラグ
    //ヒットポイントが０になった際、オブジェクトをDestroyするが、ドロップ前にオブジェクトが消えないように設ける
    //Destroyを呼ぶ前にこれを評価すること
    bool dropped = false;

    //ボーナスエネミーがドロップする資源の最大個数
    const short bonusEnemyDropResourceNumMax = 4;

    //ボーナスエネミーがドロップする資源の最小個数
    const short bonusEnemyDropResourceNumMin = 3;

    //通常エネミーがドロップする資源の最大個数
    const short normalEnemyDropResourceNumMax = 2;

    //通常エネミーがドロップする資源の最小個数
    const short normalEnemyDropResourceNumMin = 1;

    //アイテムドロップ時に物理的に放り投げる強さ
    const float dropAddForceStlength = 500.0f;

    //資源のプレハブデータ
    //スポーン時にEnemyManagerManagementスクリプトから取得済み
    GameObject resourcePrefab;

    //各武器のプレハブデータ
    //資源プレハブデータと同じく別スクリプトにて取得済み
    GameObject hammerPrefab;
    GameObject gatlingPrefab;
    GameObject railgunPrefab;

    //武器格納
    public GameObject[]weaponPrefeb;

    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStatus.hitPoint <= 0 && !dropped)
        {
            //ドロップの挙動を記述する
            if (enemyStatus.GetBonusEnemyFlag())
            {
                //ボーナスエネミーのドロップの挙動を記述する
                DropItems(bonusEnemyDropResourceNumMin, bonusEnemyDropResourceNumMax, 0, 0);
                //武器をドロップする
                DropWeapon();
            }
            else
            {
                //通常エネミーのドロップの挙動を記述する
                DropItems(normalEnemyDropResourceNumMin, normalEnemyDropResourceNumMax, 0, 0);
            }

            //ドロップのフラグを立てる
            dropped = true;
        }
    }

    //droppedフラグのゲッター
    public bool GetDroppedFlag()
    {
        return dropped;
    }

    void DropItems(int resourceNumMin, int resourceNumMax, int weaponNumMin, int weaponNumMax)
    {
        //資源を何個落とすかを設定
        int dropResourceNum = Random.Range(resourceNumMin, resourceNumMax + 1);

        //資源のインスタンス生成
        GameObject[] resource = new GameObject[dropResourceNum];
        for (int i = 0; i < dropResourceNum; i++)
        {
            resource[i] = GameObject.Instantiate(resourcePrefab, gameObject.transform.position, Quaternion.identity);
        }

        //資源を物理的に飛ばすため、資源ゲームオブジェクトのリジッドボディを取得
        Rigidbody[] resourceRigidbody = new Rigidbody[dropResourceNum];
        for (int i = 0; i < dropResourceNum; i++)
        {
            resourceRigidbody[i] = resource[i].GetComponent<Rigidbody>();
        }

        //資源を物理的に飛ばす
        for (int i = 0; i < dropResourceNum; i++)
        {
            //飛ばす角度を計算
            float angle = Mathf.PI * 2 / dropResourceNum * i;

            //飛ばす強さを設定
            //この時各方向の成分は取りうる最大値である
            //Vector3 dropDirection = new Vector3(1, Mathf.Sin(Mathf.PI / 4), 1) * dropAddForceStlength;
            Vector3 dropDirection = new Vector3(1, 0.001f, 1) * dropAddForceStlength;

            //角度に応じた値にx,z成分を設定する
            dropDirection.x *= Mathf.Sin(angle);
            dropDirection.z *= Mathf.Cos(angle);

            //ここでやっと飛ばす
            resourceRigidbody[i].AddForce(dropDirection);
        }
    }

    //資源プレハブデータのセッター
    public void SetResourcePrefab(GameObject in_resourcePrefab)
    {
        resourcePrefab = in_resourcePrefab;
        if (resourcePrefab == null)
        {
            Debug.Log("資源プレハブのデータ取得に失敗");
        }
    }

    //武器：ハンマープレハブデータのセッター
    public void SetHammerPrefab(GameObject in_hammerPrefab)
    {
        hammerPrefab = in_hammerPrefab;
        if (hammerPrefab == null)
        {
            Debug.Log("ハンマープレハブのデータ取得に失敗");
        }
    }

    //武器：ガトリングプレハブデータのセッター
    public void SetGatlingPrefab(GameObject in_gatlingPrefab)
    {
        gatlingPrefab = in_gatlingPrefab;
        if (gatlingPrefab == null)
        {
            Debug.Log("ガトリングプレハブのデータ取得に失敗");
        }
    }

    //武器：レールガンプレハブデータのセッター
    public void SetRailgunPrefab(GameObject in_railgunPrefab)
    {
        railgunPrefab = in_railgunPrefab;
        if (railgunPrefab == null)
        {
            Debug.Log("レールガンプレハブのデータ取得に失敗");
        }
    }
    //武器をドロップ
    public void DropWeapon()
    {
        GameObject dropWepon = weaponPrefeb[Random.Range(0, weaponPrefeb.Length)];

        Instantiate(dropWepon, transform.position + Vector3.up, Quaternion.identity);
    }
    //各武器のプレハブデータのセッターを一つの関数にまとめて使いやすく
    //ふと「これ、いる？」と思ったのでとりあえずコメント化
    /*
    public void SetWeaponsPrefab(GameObject in_hammerPrefab, GameObject in_gatlingPrefab, GameObject in_railgunPrefab)
    {

    }
    */
}
