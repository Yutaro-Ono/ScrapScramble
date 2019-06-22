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
    EnemyManagerManagement enemyManagerManagement;
    WaveManagement waveManager;

    //スポーンするエネミーの行動パターン
    EnemyPattern spawnEnemyPattern;

    //スポーンした回数
    //行動パターンの順番が決まっているときに使う
    //完全ランダムで行動パターンを定めるなら不要
    int spawnNum = 0;
    
    //スポーンした時間
    float timeFromLastSpawn;

    //スポーン間隔
    public float spawnInterval = 2.0f;

    //エネミーのプレハブデータ
    GameObject prefab;

    [SerializeField]
    float enemySpawnPosY = 10;

    // Start is called before the first frame update
    void Start()
    {
        enemyManagerManagement = GetComponent<EnemyManagerManagement>();

        waveManager = enemyManagerManagement.waveManager;

        spawnNum = 0;
        
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
        prefab = (GameObject)Resources.Load(prefabPath);

        //タイマー設定
        timeFromLastSpawn = WaveManagement.limitTime + spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //条件を満たしたらマネージャーの子にエネミーを生成
        //現在はデバッグ的にコマンド入力でスポーン
        //if (Input.GetKeyDown(KeyCode.I) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))

        //特定ウェーブ　＆＆　最後のスポーンから一定時間経過でマネージャーの子にエネミーを生成
        if ((waveManager.wave == WaveManagement.WAVE_NUM.WAVE_1_PVE || waveManager.wave == WaveManagement.WAVE_NUM.WAVE_3_PVE) && timeFromLastSpawn - waveManager.timer >= spawnInterval)
        {
            //エネミーの行動パターンをランダム設定。キャストとかの関係でちょっと読みづらいかも
            spawnEnemyPattern = (EnemyPattern)Random.Range((int)EnemyPattern.PatternA, (int)EnemyPattern.Invalid);

            //プレハブをもとにオブジェクト生成
            GameObject instance = (GameObject)Instantiate(prefab, new Vector3(Random.Range(-90.0f, 90.0f), enemySpawnPosY, Random.Range(-90.0f, 90.0f)),
                Quaternion.identity);

            //生成したオブジェクトをエネミーマネージャーの子に設定
            instance.transform.parent = transform;

            //個体に必要な情報を代入
            EnemyStatus enemyStatus = instance.GetComponent<EnemyStatus>();
            EnemyDrop enemyDrop = instance.GetComponent<EnemyDrop>();
            enemyManagerManagement.TellInformationsToEnemy(enemyStatus, enemyDrop);

            //スポーン回数の記録
            spawnNum++;

            //スポーン時間の記録
            timeFromLastSpawn = waveManager.timer;
        }
        else if (waveManager.wave == WaveManagement.WAVE_NUM.WAVE_INTERVAL)
        {
            timeFromLastSpawn = WaveManagement.limitTime + spawnInterval;
        }
    }
}
