//-------------------------------------------------------------------------------------------//
// 出撃画面の「スペースキー長押しでゲーム開始」の表示・非表示
//------------------------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayToNext : MonoBehaviour
{
    // ChoiceMenuSceneCo...が格納されているオブジェクト
    GameObject sceneManager;
    // scene管理スクリプト
    ChoiceMenuSceneController scene;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        scene = sceneManager.GetComponent<ChoiceMenuSceneController>();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(scene.GetPushStart() == true)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
