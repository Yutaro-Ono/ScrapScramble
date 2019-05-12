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
