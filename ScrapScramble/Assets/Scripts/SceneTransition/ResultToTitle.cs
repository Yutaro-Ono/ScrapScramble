using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultToTitle : MonoBehaviour
{
    Rewired.Player[] controllers = new Rewired.Player[PlayerManagement.playerNum];

    public float nowHoldTime;

    const float maxHoldTime = 2.0f;

    public float nowHoldTimeForQuitApp;

    const float maxHoldTimeForQuitApp = 2.0f; 

    private void Start()
    {
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            // その番号のプレイヤーがAIでなければ
            if (ChoiceMenuSceneController.getReady[i])
            {
                // 操作受付
                controllers[i] = Rewired.ReInput.players.GetPlayer(i);
            }
            // AIであれば
            else
            {
                controllers[i] = null;
            }
        }

        nowHoldTime = 0.0f;
        nowHoldTimeForQuitApp = 0.0f;
    }

    private void Update()
    {
        // 誰かがAボタンを押しているかどうか
        bool pressA = false;
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            // AIの場合はこの先を見ない
            if (controllers[i] == null)
            {
                continue;
            }

            // Aボタンが押されていれば即座に抜ける
            if (pressA = controllers[i].GetButton("A"))
            {
                break;
            }
        }

        // 誰かがBボタンを押しているかどうか
        bool pressB = false;
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            // AIの場合はこの先を見ない
            if (controllers[i] == null)
            {
                continue;
            }

            // Bボタンが押されていれば即座に抜ける
            if (pressB = controllers[i].GetButton("B"))
            {
                break;
            }
        }

        // Aボタンが押されている間
        // ただし、アプリ終了のカウントが0の時だけ
        if ((pressA || Input.GetKey(KeyCode.Space)) && nowHoldTimeForQuitApp == 0.0f)
        {
            nowHoldTime += Time.deltaTime;
        }
        // Aが押されていない間は減少
        else if (nowHoldTime > 0 && nowHoldTime < maxHoldTime)
        {
            nowHoldTime -= Time.deltaTime / 1.5f;

            if (nowHoldTime < 0)
            {
                nowHoldTime = 0.0f;
            }
        }

        // Bボタンが押されている間
        // ただし、タイトルシーンへのカウントが0の時だけ
        if ((pressB || Input.GetKey(KeyCode.Tab)) && nowHoldTime == 0.0f)
        {
            nowHoldTimeForQuitApp += Time.deltaTime;
        }
        // Bが押されていない間減少
        else if (nowHoldTimeForQuitApp > 0 && nowHoldTimeForQuitApp < maxHoldTimeForQuitApp)
        {
            nowHoldTimeForQuitApp -= Time.deltaTime / 1.5f;

            if (nowHoldTimeForQuitApp < 0)
            {
                nowHoldTimeForQuitApp = 0.0f;
            }
        }

        // 十分な時間Aが押されていればシーンを遷移
        if (nowHoldTime >= maxHoldTime)
        {
            BackToTitle();
        }

        // 十分な時間Bが押されていればゲームを終了
        if (nowHoldTimeForQuitApp >= maxHoldTimeForQuitApp)
        {
            // エディタ上ならこっち
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

            // スタンドアロン実行（実機での実行のことかな？）ならこっち
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
        }

        // エスケープキーが押されていればゲームを終了する関数
        CommonFunction.CheckEscapeForQuitApp();
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("New_Title");
    }

    
    public float GetMaxFillTime()
    {
        return maxHoldTime;
    }
}
