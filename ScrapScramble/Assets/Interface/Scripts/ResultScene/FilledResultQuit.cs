using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledResultQuit : MonoBehaviour
{
    // ロードバー(画像の取得用)
    Image loadBar;


    GameObject sceneManager;
    ResultToTitle result;


    float nowHoldTime;

    // ボタン押下の最大時間
    float maxHoldTime;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");

        result = sceneManager.GetComponent<ResultToTitle>();
        loadBar = GetComponent<Image>();


        // 最大時間を取得
        maxHoldTime = result.GetMaxFillTime();

        nowHoldTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        nowHoldTime = result.nowHoldTimeForQuitApp;

        if (nowHoldTime > 0.0f)
        {
            loadBar.fillAmount = nowHoldTime / maxHoldTime;
        }

    }
}
