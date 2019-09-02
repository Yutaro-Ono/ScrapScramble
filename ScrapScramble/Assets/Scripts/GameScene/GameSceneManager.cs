using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSceneManager : MonoBehaviour
{
    // パッド入力情報
    Rewired.Player[] input = new Rewired.Player[PlayerManagement.playerNum];

    //public int inputID = 1;            // プレイヤー1の入力のみ認識する

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

    // プレイヤーマネージャ
    PlayerManagement playerManager;

    // チュートリアル画面切り替えから数秒は無効
    float tutorialShowTimer;
    const float tutorialShowTime = 2.0f;

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

        }

        // チュートリアル1枚目は表示、2枚目は非表示
        tutorialObj[0].SetActive(true);
        tutorialObj[1].SetActive(false);

        // プレイヤーマネージャ取得
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManagement>();

        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            // AIなら
            if (playerManager.player[i].GetComponent<PlayerStatus>().AIFlag)
            {
                input[i] = null;
            }
            // プレイヤーなら
            else
            {
                input[i] = Rewired.ReInput.players.GetPlayer(i);
            }
        }

        tutorialShowTimer = 0.0f;
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
        tutorialShowTimer += Time.deltaTime;

        // Aボタンの確認
        bool pressDownA = false;
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            if (input[i] == null)
            {
                continue;
            }

            // Aボタンが押されたループを抜ける
            if (pressDownA = input[i].GetButtonDown("A"))
            {
                break;
            }
        }

        // チュートリアル1枚目の表示
        if (tutorial[0] == true)
        {

            Debug.Log("チュートリアル1");

            // Aボタン入力を確認したら演出を開始し、チュートリアル2に移行
            if ((Input.GetKeyDown(KeyCode.Space) || pressDownA) && tutorialShowTimer > tutorialShowTime)
            {
                toNext = true;
                tutorialShowTimer = 0.0f;
            }

            if (toNext == true)
            {
                tutorial[1] = transAnim.PlayTutorialProduct(true);
                
                // タイマーを初期化しtoNextをfalse
                if(tutorial[1] == true)
                {
                    tutorialObj[0].SetActive(false);
                    transAnim.InitTimer();
                    toNext = false;
                }
            }
        }

        // チュートリアル2枚目の表示
        if (tutorial[1] == true)
        {
            tutorial[0] = false;
            tutorialObj[1].SetActive(true);

            // Aボタン入力を確認したら演出を開始し、ゲーム開始のフラグを返す
            if ((Input.GetKeyDown(KeyCode.Space) || pressDownA) && tutorialShowTimer > tutorialShowTime)
            {
                toNext = true;
                tutorialShowTimer = 0.0f;
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
