using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledResult : MonoBehaviour
{

    // ロードバー(画像の取得用)
    Image loadBar;


    GameObject sceneManager;
    ResultToTitle result;


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
    }

    // Update is called once per frame
    void Update()
    {
        loadBar.fillAmount = result.nowHoldTime / maxHoldTime;
    }
}
