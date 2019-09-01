//---------------------------------------------------------------------------//
// 出撃画面でのプレイヤー準備状態をUIに反映する
//--------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationChanger : MonoBehaviour
{
    // ChoiceMenuSceneCo...が格納されているオブジェクト
    GameObject sceneManager;
    // scene管理スクリプト
    ChoiceMenuSceneController scene;

    // 何Pか(現段階ではInspector上で番号を指定)
    public int playerNum;

    // 全プレイヤー数
    public const int playerAllNum = 4;

    // 準備状態UI格納用
    public GameObject[] playerReady = new GameObject[playerAllNum];

    // 待機状態UI格納用
    public GameObject[] playerWait = new GameObject[playerAllNum];

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        scene = sceneManager.GetComponent<ChoiceMenuSceneController>();

        // プレイヤーの準備状態と待機状態のオブジェクトを取得
        for(int i = 0; i < playerAllNum; ++i)
        {
            playerReady[i] = GameObject.Find("ReadyPlayer_" + (i + 1)).gameObject;
            playerReady[i].SetActive(true);

            playerWait[i] = GameObject.Find("WaitPlayer_" + (i + 1)).gameObject;
            playerWait[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
