using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSceneManager : MonoBehaviour
{
    //// パッド入力情報
    //Rewired.Player input;

    public int inputID = 1;            // プレイヤー1の入力のみ認識する

    public const int allTutorialNum = 2;

    // チュートリアル表示フラグ(1枚目、2枚目)
    bool[] tutorial = new bool[2];
    // ゲーム開始フラグ
    bool isGameStart;
    // ゲーム終了フラグ
    bool isGameEnd;

    // 次のシーンへ移行するフラグ
    bool toNext;

    // チュートリアルイメージを持つオブジェクト
    public GameObject[] tutorialObj = new GameObject[2];

    // WaveManager
    public GameObject wave;
    // 画面演出
    public TransWaveProduct transAnim;

    // Start is called before the first frame update
    void Start()
    {
        // フラグの初期化
        tutorial[0] = true;
        tutorial[1] = false;

        toNext = false;

        //input = Rewired.ReInput.players.GetPlayer(inputID);

        // WaveManagerの取得
        wave = GameObject.Find("WaveManager");

        // 演出スクリプトの取得
        transAnim = wave.GetComponent<TransWaveProduct>();

        for (int i = 0; i < allTutorialNum; i++)
        {
            // チュートリアルを取得
            tutorialObj[i] = GameObject.Find("TutorialPic_" + (i + 1)).gameObject;
            // チュートリアルを隠す
            tutorialObj[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(tutorial[0] == true)
        {
            tutorialObj[0].SetActive(true);

            Debug.Log("チュートリアル1");

            // Aボタン入力を確認したら演出を開始し、チュートリアル2に移行
            if(Input.GetKeyDown(KeyCode.Space))
            {
                toNext = true;
            }

            if(toNext == true)
            {
                toNext = tutorial[0] = transAnim.PlayTutorialProduct(true);
            }

        }


        if (tutorial[1] == true)
        {
            tutorialObj[0].SetActive(false);
            tutorialObj[1].SetActive(true);

            transAnim.PlayIntervalProduct();


        }


        //if(isGameStart == true)
        //{

        //}

    }
}
