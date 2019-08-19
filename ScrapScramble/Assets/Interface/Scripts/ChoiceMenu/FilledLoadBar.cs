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

    // ChoiceMenuSceneCo...が格納されているオブジェクト
    GameObject sceneManager;
    // scene管理スクリプト
    ChoiceMenuSceneController scene;

    // 開始時処理
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        scene = sceneManager.GetComponent<ChoiceMenuSceneController>();
        loadBar = GetComponent<Image>();

        nowHoldTime = 0.0f;
    }

    // 更新処理
    void Update()
    {
        // 1Pの準備が完了している状態の時、スペースキーを長押しでゲーム開始
        if(Input.GetKey(KeyCode.Space)　&& scene.GetPushStart() == true)
        {
            if(nowHoldTime <= maxHoldTime)
            {
                nowHoldTime += 1.5f * Time.deltaTime;
            }
        }
        else if(nowHoldTime < maxHoldTime && nowHoldTime > 0.0f)    // 途中で指を離すとバーが減少する
        {
            nowHoldTime -= 1.0f * Time.deltaTime;
        }

        // ロードゲージ描画
        loadBar.fillAmount = nowHoldTime / maxHoldTime;

        // ロードバーが十分だったら次のシーンへ
        if (nowHoldTime >= maxHoldTime)
        {
            scene.SetStartGame(true);
        }
    }
}
