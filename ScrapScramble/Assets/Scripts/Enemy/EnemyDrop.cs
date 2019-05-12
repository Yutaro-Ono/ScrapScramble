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

    //資源のプレハブのパス
    const string resourcePrefabPath = "Prefabs/Item/Resource/Resource";

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

                //資源を何個落とすかを設定
                int dropResourceNum = Random.Range(bonusEnemyDropResourceNumMin, bonusEnemyDropResourceNumMax + 1);

                //資源のプレハブを取得
                GameObject resourcePrefab = (GameObject)Resources.Load(resourcePrefabPath);

                //資源のインスタンス生成
                GameObject[] resource = new GameObject[dropResourceNum];
                for (int i = 0; i < dropResourceNum; i++)
                {
                    resource[i] = GameObject.Instantiate(resourcePrefab, gameObject.transform);
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
                    //次はここからコーディング！
                    //resourceRigidbody[i].AddForce()
                }
            }
            else
            {
                //通常エネミーのドロップの挙動を記述する
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
}
