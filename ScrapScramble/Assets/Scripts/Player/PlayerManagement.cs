using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    // プレイヤーは（AI含めて）何人いるか
    public const uint playerNum = 4;
    public GameObject[] player = new GameObject[playerNum];

    // AIの人数（？）
    // いずれ使うことになると思う
    uint AINum;

    // プレイヤーモデルの名称テンプレート
    string PlayerModelObjectNameTemplate = "Player_";

    // プレイヤーオブジェクト内に格納されたモデルオブジェクトの数
    uint modelObjectNum = 4;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの見た目の設定
        // プレイヤーオブジェクト内のモデルオブジェクトを、プレイヤー番号に対応したものだけアクティブ化し、
        // それ以外を非アクティブ化する
        for (int lplayerNum = 0; lplayerNum < playerNum; lplayerNum++)
        {
            for (int lmodelNum = 0; lmodelNum < modelObjectNum; lmodelNum++)
            {
                // 検索するプレイヤーモデル名を設定
                string modelObjName = PlayerModelObjectNameTemplate + (lmodelNum + 1);

                // アクティブ化するのは「プレイヤー番号」と「モデル名の番号 - 1」が等しい時
                bool active = (lplayerNum == lmodelNum);

                // 検索、およびアクティブor非アクティブ化
                GameObject modelObj = (player[lplayerNum].transform.Find(modelObjName)).gameObject;
                if (modelObj == null)
                {
                    Debug.Log("プレイヤー" + (lplayerNum + 1) + "：" + modelObjName + "の取得に失敗");
                }

                modelObj.SetActive(active);
            }
        }
    }
    
    // AIの人数のセッター
    // 返却値->最終的にセットされたAIの人数
    public uint SetAINum(uint value)
    {
        AINum = value;
        return AINum;
    }
}
