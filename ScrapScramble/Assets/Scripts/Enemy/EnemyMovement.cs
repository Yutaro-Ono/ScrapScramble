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
    
    //個体が移動する際の目的地の座標
    Vector3 destination;
    Vector3 destinationDirection;
    
    //個体の移動スピード(/フレーム)
    public const float moveSpeed = 1.0f;

    //ステージの隅っこの一つとその対角の座標をもったオブジェクト
    GameObject stageCorner1, stageCorner2;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        //次工程行くかのフラグを更新
        goNextProcessFlag = false;

        //個体のHP残っているなら
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
                    cornerXLarger = stageCorner1.transform.position.x;
                    cornerXSmaller = stageCorner2.transform.position.x;

                    //間違っていたら値入れ替え
                    if (cornerXSmaller > cornerXLarger)
                    {
                        float tmp = cornerXLarger;
                        cornerXLarger = cornerXSmaller;
                        cornerXSmaller = tmp;
                    }
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
                }

                //ランダム生成
                destinationX = Random.Range(cornerXSmaller, cornerXLarger);
                destinationZ = Random.Range(cornerZSmaller, cornerZLarger);

                //目的地を示す変数に代入
                destination = new Vector3(destinationX, gameObject.transform.position.y, destinationZ);
                destinationDirection = destination.normalized;

                //次の工程へ
                nowProcess++;
            }
            else if (nowProcess == EnemyMovementProcess.Move)
            {
                //現在地と目的地との距離の計測
                float length = Vector3.Distance(destination, gameObject.transform.position);

                //目的地まで十分な距離がある場合は目的の方向まで一定スピードで進む
                if (length > Vector3.Magnitude(destinationDirection * moveSpeed))
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
    public void SetStageCornerObject(GameObject cornerObject1, GameObject cornerObject2)
    {
        stageCorner1 = cornerObject1;
        stageCorner2 = cornerObject2;
    }
}
