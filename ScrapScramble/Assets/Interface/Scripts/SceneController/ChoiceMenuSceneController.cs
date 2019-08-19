//--------------------------------------------------------------------------------//
// 出撃画面のコントローラー(scene遷移の処理を担当)
//-------------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceMenuSceneController : MonoBehaviour
{
    //選択されたプレイ人数
    //staticによってシーン遷移後もこの変数は残る(ChoiceMenuSceneController.playerNumでアクセス可能)
    public static int playerNum = 0;

    // 最大プレイ人数
    const int maxPlayer = 4;

    // プレイヤーの準備状況を格納
    bool[] getReady = new bool[maxPlayer];

    // 次のシーンへ行けるかどうかのフラグ(これがtrueになって初めてscene遷移が行える)
    private bool pushStart;

    // 次のシーン(ゲームを開始するかどうかのフラグ)
    private bool startGame;

    // scene開始時処理
    void Start()
    {
        //// プレイヤーの準備状況を初期化
        //for(int i = 0; i < maxPlayer; i++)
        //{
        //    getReady[i] = false;
        //}

        // ※デバッグ用初期化(準備状況が最初からtrue)※
        for (int i = 0; i < maxPlayer; i++)
        {
            getReady[i] = true;
        }

        // プレイ人数を初期化
        playerNum = 0;

        //pushStart = false;
        // ※デバッグ用初期化(1Pが完了していなくてもスタート可能)※
        pushStart = true;
        startGame = false;
    }

    // 更新処理
    void Update()
    {

        // ゲーム開始フラグが立ったら
        if(startGame == true)
        {
            // プレイヤーの準備状況がtrueだった場合、プレイ人数を加算
            for(int i = 0; i < maxPlayer; i++)
            {
                if(getReady[i] == true)
                {
                    playerNum++;
                }
            }
            // 次のシーンへ
            SceneTransition();
        }

        // プレイヤーの準備状況を更新
        CheckReady();
    }

    //-------------------------------------------------------------------------------------------------------------//
    // 関数
    //-------------------------------------------------------------------------------------------------------------//
    // プレイヤー毎のパッド入力情報をチェックし、準備状況を更新 ※input関連の環境が整ったら実装する
    private void CheckReady()
    {
        // プレイヤーの準備状況を更新
        for (int i = 0; i < maxPlayer; i++)
        {

        }

        // 1Pの準備が完了していたらゲーム開始を可能とする(1Pが起点)
        if (getReady[0] == true)
        {
            pushStart = true;
        }
        else
        {
            // ※デバッグ用処理ストップ※
            // pushStart = false;
        }
    }

    // ゲームシーンへ遷移させる
    public void SceneTransition()
    {
        SceneManager.LoadScene("BattleScene");
    }


    //------------------------------------------------------------------------------------------------------------//
    // ゲッター / セッター
    //-----------------------------------------------------------------------------------------------------------//
    // 準備完了したプレイヤーの人数のゲッター
    public int GetPlayerNum()
    {
        return playerNum;
    }

    // プレイヤーの準備が完了しているかどうかのゲッター
    public bool GetPushStart()
    {
        return pushStart;
    }

    // プレイヤーの準備状態ゲッター
    public bool GetPlayerReady(int in_playerNum)
    {
        return getReady[in_playerNum];
    }

    // ゲームをスタートするかどうかのセッター
    public void SetStartGame(bool in_start)
    {
        startGame = in_start;
    }

}
