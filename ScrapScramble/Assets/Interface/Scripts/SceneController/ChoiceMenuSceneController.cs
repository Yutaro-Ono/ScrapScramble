//--------------------------------------------------------------------------------//
// 出撃画面のコントローラー
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

    // Start is called before the first frame update
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
            getReady[i] = false;
        }

        //pushStart = false;
        // ※デバッグ用初期化(1Pが完了していなくてもスタート可能)※
        pushStart = true;
        startGame = false;
    }

    // Update is called once per frame
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

    // プレイヤーの準備が完了しているかどうかのゲッター
    public bool GetPushStart()
    {
        return pushStart;
    }

    // ゲームをスタートするかどうかのセッター
    public void SetStartGame(bool in_start)
    {
        startGame = in_start;
    }

    // ゲームシーンへ遷移させる
    public void SceneTransition()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
