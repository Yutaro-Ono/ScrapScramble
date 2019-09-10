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
    public GameObject playerReady;

    // 待機状態UI格納用
    public GameObject playerWait;

    // 対応するコントローラ
    Rewired.Player controller;

    // プレイヤー（AIでない）として参加するかどうか
    bool readyFlag;

    // 準備状態になった押下であるか
    bool holdFromBecomeReady;

    // 次シーンへ遷移する操作をしたかどうか
    bool toNextSceneCommand;

    public bool playReadySoundFlag;
    public bool playWaitSoundFlag;

    // Start is called before the first frame update
    void Start()
    {
        readyFlag = false;
        holdFromBecomeReady = false;
        toNextSceneCommand = false;

        playReadySoundFlag = false;
        playWaitSoundFlag = false;

        sceneManager = GameObject.Find("SceneManager");
        scene = sceneManager.GetComponent<ChoiceMenuSceneController>();

        // プレイヤーの準備状態と待機状態のオブジェクトを取得
        int number = playerNum + 1;
        playerReady = transform.Find("ReadyPlayer_" + number).gameObject;
        playerWait = transform.Find("WaitPlayer_" + number).gameObject;
        playerReady.SetActive(false);
        playerWait.SetActive(true);

        // コントローラ情報を取得
        controller = Rewired.ReInput.players.GetPlayer(playerNum);
    }

    // Update is called once per frame
    void Update()
    {
        // Aボタンを押したとき
        if (controller.GetButton("A"))
        {
            // 準備フラグが偽であるとき、準備状態にする
            if (!readyFlag)
            {
                readyFlag = true;
                holdFromBecomeReady = true;
                playerReady.SetActive(true);
                playerWait.SetActive(false);

                playReadySoundFlag = true;
            }

            // 準備フラグが真で、準備フラグが立った押下でなければ（準備状態になって、一度離された上で押されたなら）
            else if (!holdFromBecomeReady)
            {
                toNextSceneCommand = true;
            }
        }

        // Aボタンが押されていないとき
        else
        {
            holdFromBecomeReady = false;
            
            toNextSceneCommand = false;
        }

        // Bボタンが押されていれば、準備状態をキャンセル
        if (controller.GetButton("B"))
        {
            if (readyFlag)
            {
                playWaitSoundFlag = true;
            }

            readyFlag = false;
            playerReady.SetActive(false);
            playerWait.SetActive(true);
        }
    }

    public bool GetReadyFlag()
    {
        return readyFlag;
    }

    public bool GetToNextSceneCommand()
    {
        return toNextSceneCommand;
    }
}
