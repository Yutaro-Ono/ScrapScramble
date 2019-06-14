using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovementProcess
{
    DecideDestination,
    Move,
    Targeting,
    Shoot,
    Invalid,
}

public class EnemyMovement : MonoBehaviour
{
    EnemyStatus enemyStatus;

    //行動ルーチンのうち、現在履行中の工程を格納
    EnemyMovementProcess nowProcess = (EnemyMovementProcess)0;

    //次の工程に行くかどうかのフラグ
    bool goNextProcessFlag;

    //撃破演出が終わったかのフラグ
    bool finishBusteredAnimationFlag = false;

    ///////////////////////////////////
    //行き先の決定、移動に関わる変数
    ///////////////////////////////////

    //個体が移動する際の目的地の座標と、自身から見たその相対ベクトルを正規化したものを格納
    Vector3 destination;
    Vector3 destinationDirection;
    
    //個体の移動スピード(/フレーム)
    const float moveSpeed = 0.3f;

    //ステージの隅っこの一つとその対角の座標をもったオブジェクト
    Transform stageCorner1, stageCorner2;

    //ステージの境界の壁からどのくらい離れた位置に移動させるか
    //移動した後に壁と取っている距離
    const float rangeTakenWithWall = 10.0f;

    ///////////////////////////////////
    //ターゲッティングに関わる変数
    ///////////////////////////////////

    //プレイヤーの人数（読み込むことが多いのでこちらでも格納することにしました）
    const int playerNum = EnemyManagerManagement.playerNum;

    //プレイヤーの情報
    GameObject[] playerObj = new GameObject[playerNum];

    //ターゲッティング方向
    //プレイヤーのTransformをそのまま使うとやや上方向を向いてしまうので
    Transform targetingTransform;

    //ターゲッティングする秒数
    //インスペクタからも変更可能とする
    public float targetingSeconds = 2.0f;

    //タイマー
    float targetingTimer = 0.0f;

    //ターゲットにするプレイヤーの番号
    //ターゲッティング工程にてこれが0未満の時、プレイヤーのランダム設定を行う
    int targetPlayerNumber = -1;

    ///////////////////////////////////
    //射撃に関わる変数
    ///////////////////////////////////

    //撃ち出す弾のオブジェクトのプレハブデータ
    GameObject bulletPrefab;
    const string bulletPath = "Prefabs/Enemy/Enemy's Bullet";

    //弾を撃ち出す位置の情報
    Transform shootPoint;

    //撃ち出すまでの待機時間（これがなければプレイヤーは弾を避けにくくなる）
    public float shootWaitingTime = 1.0f;

    //タイマー
    float shootTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        //エネミーのステータス取得
        enemyStatus = GetComponent<EnemyStatus>();

        //ステージの角の位置を取得
        stageCorner1 = enemyStatus.stageCorner1;
        stageCorner2 = enemyStatus.stageCorner2;

        //全プレイヤーの情報を取得
        for (int i = 0; i < playerNum; i++)
        {
            playerObj[i] = enemyStatus.player[i];
        }

        //弾のプレハブを取得
        bulletPrefab = (GameObject)Resources.Load(bulletPath);
        if (bulletPrefab == null)
        {
            Debug.Log("エネミー：弾のプレハブデータ取得に失敗しました");
        }

        //弾を発射する座標を取得
        shootPoint = transform.Find("Shoot Point");
    }

    // Update is called once per frame
    void Update()
    {
        

        //次工程に行くかのフラグを更新
        goNextProcessFlag = false;

        
        //個体のHPが残っているなら
        if (enemyStatus.hitPoint > 0)
        {
            /////////////////////////////////////
            //目的地決定プロセス
            /////////////////////////////////////
            if (nowProcess == EnemyMovementProcess.DecideDestination)
            {

                //行き先のランダム設定
                SetDestinationAtRandom();

                //方向ベクトルの格納
                SetDestinationDirection();
                
                //次の工程へ
                nowProcess++;

            }
            /////////////////////////////////////
            //移動プロセス
            /////////////////////////////////////
            else if (nowProcess == EnemyMovementProcess.Move)
            {
                
                //進む方向を取得しなおす
                SetDestinationDirection();


                //進む方向を見る
                gameObject.transform.LookAt(destination);


                //現在地と目的地との距離の計測
                float distance = Vector3.Distance(destination, gameObject.transform.position);
                float moveLength = Vector3.Magnitude(destinationDirection * moveSpeed);


                //目的地まで十分な距離がある場合は目的の方向まで一定スピードで進む
                if (distance > moveLength)
                {

                    gameObject.transform.position += destinationDirection * moveSpeed;

                }
                //そうでないときは目的地の座標を自身の座標に代入
                else
                {


                    gameObject.transform.position = destination;
                    goNextProcessFlag = true;


                }


                CheckGoNextProcessFlag();


            }
            /////////////////////////////////////
            //ターゲッティングプロセス
            /////////////////////////////////////
            else if (nowProcess == EnemyMovementProcess.Targeting)
            {

                //ターゲッティング対象が未決定の時、対象をランダム設定
                if (targetPlayerNumber < 0)
                {
                    targetPlayerNumber = Random.Range(0, playerNum);
                    //targetPlayerNumber = 0;

                    Debug.Log("狙っているプレイヤー番号：" + targetPlayerNumber);
                }

                
                //プレイヤーの位置（y座標考えないver）を格納
                targetingTransform = playerObj[targetPlayerNumber].transform;
                //targetingTransform.position = new Vector3(playerObj[targetPlayerNumber].transform.position.x, gameObject.transform.position.y, playerObj[targetPlayerNumber].transform.position.z);
                

                //ターゲッティングしたプレイヤーの方を向く
                gameObject.transform.LookAt(targetingTransform);
                
                //ただし、x軸回転は0のまま
                {
                    //x軸回転の値を取得する
                    float xRotation = gameObject.transform.rotation.x;

                    //取得した値分逆回転させる
                    gameObject.transform.Rotate(new Vector3(-xRotation, 0, 0));
                }

                //指定した秒数ターゲッティングしたら次の工程へ
                targetingTimer += Time.deltaTime;

                if (targetingTimer >= targetingSeconds)
                {
                    Debug.Log("ターゲッティング完了");

                    goNextProcessFlag = true;
                }

                CheckGoNextProcessFlag();


            }
            /////////////////////////////////////
            //発射プロセス
            /////////////////////////////////////
            else if (nowProcess == EnemyMovementProcess.Shoot)
            {

                //発砲待機時間の計測
                shootTimer += Time.deltaTime;

                if (shootTimer >= shootWaitingTime)
                {

                    //弾のインスタンスを生成
                    GameObject bulletInstance = (GameObject)Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);

                    //弾にプレイヤー情報を教える
                    BulletOfEnemyMovement bulletScript = bulletInstance.GetComponent<BulletOfEnemyMovement>();
                    bulletScript.SetPlayerObject(playerObj, playerNum);

                    Debug.Log("バン！：エネミーがプレイヤーに向けて発砲した");


                    goNextProcessFlag = true;

                }

                CheckGoNextProcessFlag();


            }
            /////////////////////////////////////
            //全工程終了プロセス
            /////////////////////////////////////
            else if (nowProcess >= EnemyMovementProcess.Invalid)
            {

                //初期化を行う
                targetingTimer = 0.0f;
                targetPlayerNumber = -1;

                shootTimer = 0.0f;

                //一番最初の工程に戻る
                nowProcess = (EnemyMovementProcess)0;
                
            }
        }
        //個体が死んだら
        else
        {

        }
    }

    /////////////////////////////////
    //自作関数群
    /////////////////////////////////

    //エネミーのgoNextProcessFlagを評価し、trueなら次の工程へ
    bool CheckGoNextProcessFlag()
    {
        if (goNextProcessFlag)
        {
            nowProcess++;
        }

        return goNextProcessFlag;
    }

    //ステージの隅っこの座標を持ったオブジェクトを格納するための関数
    public void SetStageCornerObject(Transform cornerObject1, Transform cornerObject2)
    {
        stageCorner1 = cornerObject1;
        stageCorner2 = cornerObject2;
    }

    //エネミーの移動先をランダムに設定する
    void SetDestinationAtRandom()
    {

        //目的地の座標
        float destinationX, destinationZ;


        //各隅っこの座標
        //目的地座標のランダム生成に使う
        float cornerXLarger, cornerXSmaller;
        float cornerZLarger, cornerZSmaller;


        //隅っこのx座標の大小を比較
        {


            //とりあえず代入
            cornerXLarger = stageCorner1.position.x;
            cornerXSmaller = stageCorner2.position.x;


            //間違っていたら値入れ替え
            if (cornerXSmaller > cornerXLarger)
            {


                float tmp = cornerXLarger;
                cornerXLarger = cornerXSmaller;
                cornerXSmaller = tmp;


            }


            //rangeTakenWithWall分だけ壁と距離を取る
            cornerXLarger -= rangeTakenWithWall;
            cornerXSmaller += rangeTakenWithWall;


        }

        //隅っこのz座標の大小を比較
        {


            //とりあえず代入
            cornerZLarger = stageCorner1.transform.position.z;
            cornerZSmaller = stageCorner2.transform.position.z;


            //間違っていたら値入れ替え
            if (cornerZSmaller > cornerZLarger)
            {


                float tmp = cornerZLarger;
                cornerZLarger = cornerZSmaller;
                cornerZSmaller = tmp;


            }


            //rangeTakenWithWall分だけ壁と距離を取る
            cornerZLarger -= rangeTakenWithWall;
            cornerZSmaller += rangeTakenWithWall;


        }



        //ランダム生成
        destinationX = Random.Range(cornerXSmaller, cornerXLarger);
        destinationZ = Random.Range(cornerZSmaller, cornerZLarger);


        //目的地を示す変数に代入
        destination = new Vector3(destinationX, gameObject.transform.position.y, destinationZ);
    }

    //エネミーが動く方向ベクトルを正規化して格納
    void SetDestinationDirection()
    {
        destinationDirection = (destination - gameObject.transform.position).normalized;
    }

    //finishBusteredAnimationFlagのゲッター
    public bool GetFinishBusteredFlag()
    {
        return finishBusteredAnimationFlag;
    }
}
