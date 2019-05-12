using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyPattern
{
    PatternA,
    PatternB,
    PatternC,
    Invalid
}

public class EnemyManagerSpawn : MonoBehaviour
{
    //スポーンするエネミーの行動パターン
    EnemyPattern spawnEnemyPattern;

    //スポーンした回数
    //行動パターンの順番が決まっているときに使う
    //完全ランダムで行動パターンを定めるなら不要
    int spawnNum = 0;

    //ゲームスタートからの時間
    float timeFromStart;

    // Start is called before the first frame update
    void Start()
    {
        spawnNum = 0;

        timeFromStart = 0.00f;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム開始からの時間の計測
        timeFromStart += Time.deltaTime;

        //条件を満たしたらマネージャーの子にエネミーを生成
        //現在はデバッグ的にコマンド入力でスポーン
        if (Input.GetKeyDown(KeyCode.I) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            //エネミーの行動パターンをランダム設定。キャストとかの関係でちょっと読みづらいかも
            spawnEnemyPattern = (EnemyPattern)Random.Range((int)EnemyPattern.PatternA, (int)EnemyPattern.Invalid);

            //生成するプレハブのパスを設定。
            string prefabPath;
            if (spawnEnemyPattern != EnemyPattern.Invalid)
            {
                prefabPath = "Prefabs/Enemy/Enemy";
            }
            else
            {
                prefabPath = null;
                Debug.Log("エネミーのインスタンス生成に失敗");
            }

            //プレハブ取得
            GameObject prefab = (GameObject)Resources.Load(prefabPath);

            //プレハブをもとにオブジェクト生成
            GameObject instance = (GameObject)Instantiate(prefab, new Vector3(Random.Range(-90.0f, 90.0f), 4, Random.Range(-90.0f, 90.0f)),
                Quaternion.identity);

            //生成したオブジェクトをエネミーマネージャーの子に設定
            instance.transform.parent = transform;

            //スポーン回数の記録
            spawnNum++;
        }
    }
}
