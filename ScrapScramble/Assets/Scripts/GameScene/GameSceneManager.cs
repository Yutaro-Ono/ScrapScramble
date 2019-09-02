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
    public bool isGameStart;
    // ゲーム終了フラグ
    public bool isGameEnd;

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

        if(isGameStart == false)
        {
            TutorialUpdate();
        }

        if(isGameStart == true)
        {
            tutorial[1] = false;
            tutorialObj[1].SetActive(false);
        }
    }

    // チュートリアルでの更新処理
    void TutorialUpdate()
    {

        // チュートリアル1枚目の表示
        if (tutorial[0] == true)
        {
            tutorialObj[0].SetActive(true);

            Debug.Log("チュートリアル1");

            // Aボタン入力を確認したら演出を開始し、チュートリアル2に移行
            if (Input.GetKeyDown(KeyCode.Space))
            {
                toNext = true;
            }

            if (toNext == true)
            {
                tutorial[1] = transAnim.PlayTutorialProduct(true);
                
                // タイマーを初期化しtoNextをfalse
                if(tutorial[1] == true)
                {
                    transAnim.InitTimer();
                    toNext = false;
                }
            }
        }

        // チュートリアル2枚目の表示
        if (tutorial[1] == true)
        {
            tutorial[0] = false;
            tutorialObj[0].SetActive(false);
            tutorialObj[1].SetActive(true);

            // Aボタン入力を確認したら演出を開始し、ゲーム開始のフラグを返す
            if (Input.GetKeyDown(KeyCode.Space))
            {
                toNext = true;
            }

            if(toNext == true)
            {
                // アニメの再生が終わったらゲームを開始
                isGameStart = transAnim.PlayTutorialProduct(true);
            }
        }
    }

    // ゲーム開始したかどうかのゲッター
    public bool GetGameStartFlag()
    {
        return isGameStart;
    }

}
