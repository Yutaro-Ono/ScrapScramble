﻿using System.Collections;
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

    // ウェーブ情報
    static WaveManagement waveManager;

    // プレイヤーモデルの名称テンプレート
    string PlayerModelObjectNameTemplate = "Player_";

    // プレイヤーオブジェクト内に格納されたモデルオブジェクトの数
    uint modelObjectNum = 4;

    // Start関数よりも前に呼び出される関数
    private void Awake()
    {
        // ウェーブ情報取得
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManagement>();

        // 子のプレイヤーオブジェクトを取得
        for (int i = 0; i < playerNum; ++i)
        {
            player[i] = transform.Find("Player" + (i + 1)).gameObject;
            if (player[i] == null)
            {
                Debug.Log("プレイヤーオブジェクトの取得失敗");
            }
        }

        // プレイヤー番号（ID)を個体に設定
        for (int i = 0; i < playerNum; ++i)
        {
            PlayerStatus status = player[i].GetComponent<PlayerStatus>();

            status.SetId(i);
        }

        // AIか否かの設定
        if (ChoiceMenuSceneController.playerNum != 0)
        {
            for (int lplayerNum = 0; lplayerNum < playerNum; lplayerNum++)
            {
                PlayerStatus status = player[lplayerNum].GetComponent<PlayerStatus>();

                if (ChoiceMenuSceneController.getReady[lplayerNum])
                {
                    status.AIFlag = false;
                }
                else
                {
                    status.AIFlag = true;
                }
            }
        }
        // 参加者0なら全員AI
        else
        {
            for (int i = 0; i < playerNum; ++i)
            {
                PlayerStatus status = player[i].GetComponent<PlayerStatus>();

                status.AIFlag = true;
            }
        }
    }

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
                GameObject modelObj = (player[lplayerNum].transform.Find("Core").transform.Find(modelObjName)).gameObject;
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

    public static WaveManagement GetWaveManager()
    {
        return waveManager;
    }
}
