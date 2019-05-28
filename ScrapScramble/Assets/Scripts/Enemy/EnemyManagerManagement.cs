using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerManagement : MonoBehaviour
{
    public const int playerNum = 4;

    //プレイヤーの情報
    public GameObject[] player = new GameObject[playerNum];

    //ステージの隅っこの一つとその対角の座標をもったオブジェクト
    public Transform stageCorner1, stageCorner2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TellInformationsToEnemy(EnemyStatus status)
    {
        status.stageCorner1 = stageCorner1;
        status.stageCorner2 = stageCorner2;

        for (int i = 0; i < playerNum; i++)
        {
            status.player[i] = player[i];
        }
    }
}
