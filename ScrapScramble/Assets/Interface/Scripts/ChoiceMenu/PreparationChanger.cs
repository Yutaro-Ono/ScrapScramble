//---------------------------------------------------------------------------//
// 出撃画面でのプレイヤー準備状態をUIに反映する
//--------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationChanger : MonoBehaviour
{
    // Image(Ready)
    Image readyImg;
    GameObject ready;
    // Image(Wait)
    Image waitImg;
    GameObject wait;

    // ChoiceMenuSceneCo...が格納されているオブジェクト
    GameObject sceneManager;
    // scene管理スクリプト
    ChoiceMenuSceneController scene;

    // 何Pか(現段階ではInspector上で番号を指定)
    public int playerNum;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        scene = sceneManager.GetComponent<ChoiceMenuSceneController>();

        ready = this.transform.Find("Ready").gameObject;
        readyImg = ready.gameObject.GetComponent<Image>();

        wait = this.transform.Find("Wait").gameObject;
        waitImg = wait.gameObject.GetComponent<Image>();

        readyImg.enabled = false;
        waitImg.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // アタッチされたオブジェクトのプレイヤー番号に応じて、準備状態を取得。画像の表示非表示を切り替える
        if(scene.GetPlayerReady(playerNum) == true)
        {
            readyImg.enabled = true;
            waitImg.enabled = false;
        }
        else
        {
            readyImg.enabled = false;
            waitImg.enabled = true;
        }


    }
}
