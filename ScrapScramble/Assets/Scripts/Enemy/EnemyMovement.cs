using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovementProcess
{
    DecideDestination,
    Move,
    Aim,
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
    
    //個体が移動する際の目的地の座標とその正規化ベクトル
    Vector3 destination;
    Vector3 destinationDirection;
    
    //個体の移動スピード(/フレーム)
    const float moveSpeed = 0.3f;

    //ステージの隅っこの一つとその対角の座標をもったオブジェクト
    Transform stageCorner1, stageCorner2;

    //ステージの境界の壁からどのくらい離れた位置に移動させるか
    //移動した後に壁と取っている距離
    const float rangeTakenWithWall = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        

        //次工程に行くかのフラグを更新
        goNextProcessFlag = false;

        
        //個体のHPが残っているなら
        if (enemyStatus.hitPoint > 0)
        {

            if (nowProcess == EnemyMovementProcess.DecideDestination)
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
                destinationDirection = (destination - gameObject.transform.position).normalized;

                
                //次の工程へ
                nowProcess++;


            }
            else if (nowProcess == EnemyMovementProcess.Move)
            {


                //進む方向を取得しなおす
                destinationDirection = (destination - gameObject.transform.position).normalized;


                //現在地と目的地との距離の計測
                float distance = Vector3.Distance(destination, gameObject.transform.position);
                float moveLength = Vector3.Magnitude(destinationDirection * moveSpeed);

                Debug.Log("MoveLength : " + moveLength + ", Distance : " + distance);

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
            else if (nowProcess == EnemyMovementProcess.Aim)
            {


                CheckGoNextProcessFlag();


            }
            else if (nowProcess == EnemyMovementProcess.Shoot)
            {


                CheckGoNextProcessFlag();


            }
            else
            {


                nowProcess = (EnemyMovementProcess)0;


            }
        }
        //個体が死んだら
        else
        {

        }
    }

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

    //finishBusteredAnimationFlagのゲッター
    public bool GetFinishBusteredFlag()
    {
        return finishBusteredAnimationFlag;
    }
}
