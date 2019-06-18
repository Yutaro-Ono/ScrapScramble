using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerManagement : MonoBehaviour
{
    public WaveManagement waveManager;

    public const int playerNum = 4;

    //プレイヤーの情報
    public GameObject[] player = new GameObject[playerNum];

    //ステージの隅っこの一つとその対角の座標をもったオブジェクト
    public Transform stageCorner1, stageCorner2;

    //資源のプレハブデータ
    //スポーンしたエネミーに渡す
    GameObject resourcePrefab;

    //資源のプレハブのパス
    const string resourcePrefabPath = "Prefabs/Item/Resource/Resource";

    // Start is called before the first frame update
    void Start()
    {
        resourcePrefab = (GameObject)Resources.Load(resourcePrefabPath);
    }

    // Update is called once per frame
    void Update()
    {
        //エネミーがいるべきウェーブでない場合、このエネミーオブジェクトを全削除
        if (waveManager.wave != WaveManagement.WAVE_NUM.WAVE_1_PVE && waveManager.wave != WaveManagement.WAVE_NUM.WAVE_3_PVE)
        {
            foreach(Transform n in gameObject.transform)
            {
                Destroy(n.gameObject);
            }
        }
    }

    public void TellInformationsToEnemy(EnemyStatus status, EnemyDrop drop)
    {
        //ステージ隅の情報を格納
        status.stageCorner1 = stageCorner1;
        status.stageCorner2 = stageCorner2;

        //プレイヤーの情報を格納
        for (int i = 0; i < playerNum; i++)
        {
            status.player[i] = player[i];
        }

        //ドロップする資源、武器のプレハブデータ格納
        drop.SetResourcePrefab(resourcePrefab);
    }
}
