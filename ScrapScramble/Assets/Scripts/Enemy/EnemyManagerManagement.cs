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

    //各武器のプレハブデータ
    //上と同じく
    GameObject hammerPrefab;
    GameObject gatlingPrefab;
    GameObject railgunPrefab;

    //資源のプレハブのパス
    const string resourcePrefabPath = "Prefabs/Item/Resource/Resource";

    //武器のプレハブデータを格納しているフォルダのディレクトリ
    const string weaponPrefabFolderDirectory = "Prefabs/Item/Weapon/";

    //エネミーAIの停止
    public bool stopAIFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        resourcePrefab = (GameObject)Resources.Load(resourcePrefabPath);

        hammerPrefab = (GameObject)Resources.Load(weaponPrefabFolderDirectory + "Hammer/Hammer");
        gatlingPrefab = (GameObject)Resources.Load(weaponPrefabFolderDirectory + "Gatling/Gatling");
        railgunPrefab = (GameObject)Resources.Load(weaponPrefabFolderDirectory + "Railgun/Railgun");
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

        //AI停止フラグのコピー
        foreach(Transform n in gameObject.transform)
        {
            EnemyMovement movement = n.GetComponent<EnemyMovement>();
            movement.stopAIFlag = this.stopAIFlag;
        }
    }

    public void TellInformationsToEnemy(EnemyStatus status, EnemyDrop drop)
    {
        //ステージ隅の情報を格納
        status.stageCorner1 = this.stageCorner1;
        status.stageCorner2 = this.stageCorner2;

        //プレイヤーの情報を格納
        for (int i = 0; i < playerNum; i++)
        {
            status.player[i] = this.player[i];
        }

        //ドロップする資源、武器のプレハブデータ格納
        // 資源
        drop.SetResourcePrefab(resourcePrefab);

        //武器
        drop.SetHammerPrefab(hammerPrefab);
        drop.SetGatlingPrefab(gatlingPrefab);
        drop.SetRailgunPrefab(gatlingPrefab);
    }
}
