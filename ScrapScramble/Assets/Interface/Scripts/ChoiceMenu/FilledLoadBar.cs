//--------------------------------------------------------------------------------//
// 出撃画面のロードメーター操作
//-------------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledLoadBar : MonoBehaviour
{
    Image loadBar;
    // 最大長押し時間(これを超すと次のシーンへ)
    float maxHoldTime = 3.0f;
    // 現在の長押し時間
    float nowHoldTime;

    void Start()
    {
        loadBar = GetComponent<Image>();
        nowHoldTime = 0.0f;
    }

    void Update()
    {
        // スペースキーを長押しでゲーム開始
        if(Input.GetKey(KeyCode.Space))
        {
            if(nowHoldTime <= maxHoldTime)
            {
                nowHoldTime += 1.0f * Time.deltaTime;
            }
            // ロードゲージ描画
            loadBar.fillAmount = nowHoldTime / maxHoldTime;
        }
        else if(nowHoldTime < maxHoldTime)
        {
            nowHoldTime = 0.0f;
        }
    }
}
