using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultToTitle : MonoBehaviour
{
    Rewired.Player[] controllers = new Rewired.Player[PlayerManagement.playerNum];

    public float nowHoldTime;

    const float maxHoldTime = 2.0f;

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

        // Aボタンが押されている間
        if (pressA || Input.GetKey(KeyCode.Space))
        {
            nowHoldTime += Time.deltaTime;
        }
        // Aが押されていない間は減少
        else if (nowHoldTime > 0 && nowHoldTime < maxHoldTime)
        {
            nowHoldTime -= Time.deltaTime / 1.5f;
        }

        // 十分な時間押されていればシーンを遷移
        if (nowHoldTime >= maxHoldTime)
        {
            BackToTitle();
        }
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
