using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    ////////////////////////////
    //自身のコンポーネント
    ////////////////////////////
    EnemyDrop enemyDrop;
    EnemyMovement enemyMovement;

    ////////////////////////////
    //状況
    ////////////////////////////
    //ステージの一角とその対角にあるオブジェクトの座標
    public Transform stageCorner1, stageCorner2;

    //全プレイヤーの情報
    public GameObject[] player = new GameObject[EnemyManagerManagement.playerNum];

    ////////////////////////////
    //自身の情報
    ////////////////////////////
    //エネミーのヒットポイントの最大値
    const short enemyHitPointMax = 50;

    //エネミーのヒットポイント
    //プレイヤーからの攻撃がヒットすると減少する
    public short hitPoint = enemyHitPointMax;

    //武器の出やすいボーナスエネミーであるかのフラグ
    //Start()関数内でランダムで決定される
    bool bonusEnemyFlag;

    //ボーナスエネミーであるかの判定に使うランダム変数の最大値
    const ushort bonusEnemyJudgeNumberMax = 100;

    //ボーナスエネミーになる確率
    //０～１００の間でランダムに生成された値が、この値未満であればボーナスエネミーである
    //とりあえず30%に設定
    const ushort bonusEnemyPercentage = (bonusEnemyJudgeNumberMax * 3) / 10;
    
    // Start is called before the first frame update
    void Start()
    {
        //判定の番号
        //０～１００の間でランダム生成
        ushort bonusEnemyJudgeNumber = (ushort)Random.Range(0, bonusEnemyJudgeNumberMax + 1);

        if (bonusEnemyJudgeNumber < bonusEnemyPercentage)
        {
            bonusEnemyFlag = true;
            Debug.Log("ボーナスエネミー : " + bonusEnemyJudgeNumber);
        }
        else
        {
            bonusEnemyFlag = false;
            Debug.Log("ノーマルエネミー : " + bonusEnemyJudgeNumber);
        }

        //同個体のドロップスクリプトと同期
        enemyDrop = GetComponent<EnemyDrop>();

        //同個体のムーブメントスクリプトと同期
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ的に、コマンド入力でエネミーHPを0に
        if (Input.GetKeyDown(KeyCode.D) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            hitPoint = 0;
        }
        
        //HP0かつドロップが完了している場合、ゲームオブジェクトを破壊する
        if (hitPoint <= 0 && enemyDrop.GetDroppedFlag())
        {
            Destroy(gameObject);
        }
    }

    //bonusEnemyFlagのゲッター
    public bool GetBonusEnemyFlag()
    {
        return bonusEnemyFlag;
    }
}
