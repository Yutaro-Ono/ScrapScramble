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

    // コントローラ操作
    PreparationChanger[] playerControllers = new PreparationChanger[PreparationChanger.playerAllNum];

    // 開始時処理
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        scene = sceneManager.GetComponent<ChoiceMenuSceneController>();
        loadBar = GetComponent<Image>();

        nowHoldTime = 0.0f;

        for (int i = 0; i < PreparationChanger.playerAllNum; ++i)
        {
            this.playerControllers[i] = ChoiceMenuSceneController.playerControllers[i];
        }
    }

    // 更新処理
    void Update()
    {
        // 次シーンへ遷移する操作がされているかのチェック  
        bool command = false;
        for (int i = 0; i < PreparationChanger.playerAllNum; ++i)
        {
            // 誰か一人でもその操作をしていれば真として、ループを抜ける
            if (command = playerControllers[i].GetToNextSceneCommand())
            {
                break;
            }
        }

        // 誰かしらの準備が完了している場合、特定操作でゲージ上昇
        if((Input.GetKey(KeyCode.Space) || command) && scene.GetPushStart() == true)
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
