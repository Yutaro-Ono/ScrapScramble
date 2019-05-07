using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerSpawn : MonoBehaviour
{
    //スポーンした回数
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

        
    }
}
