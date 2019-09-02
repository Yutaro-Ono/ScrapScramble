using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultToTitle : MonoBehaviour
{
    Rewired.Player player1;

    public float nowHoldTime;

    const float maxHoldTime = 2.0f;

    private void Start()
    {
        player1 = Rewired.ReInput.players.GetPlayer(0);

        nowHoldTime = 0.0f;
    }

    private void Update()
    {
        // Aボタンが押されている間
        if (player1.GetButton("A"))
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
        SceneManager.LoadScene("TitleScene");
    }
}
