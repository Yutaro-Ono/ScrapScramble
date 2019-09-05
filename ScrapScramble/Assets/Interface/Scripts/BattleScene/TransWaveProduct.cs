//----------------------------------------------------------------------//
// Wave遷移時の演出を管理する
// ※WaveManagerからWaveの状態を取得し、インターバル時に画面効果を加える
//---------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransWaveProduct : MonoBehaviour
{
    // WaveManagerオブジェクトを格納する
    GameObject waveManager;
    WaveManagement wave;
    // GameSceneManagerオブジェクトを格納する
    GameObject sceneManager;
    GameSceneManager scene;

    //----------------------------------------------------//
    // UI関連オブジェクト
    //---------------------------------------------------//
    // 演出用UI
    public GameObject productUI;
    // ウェーブ情報UI(親)
    public GameObject waveInfo;
    // ウェーブ情報UI(子：撃退ウェーブ、対戦ウェーブ)
    public GameObject[] waveIndex = new GameObject[2];

    Animator wallAnim;

    //------------------------------------------------------//
    // 演出制御
    //-----------------------------------------------------//
    // シーンの間隔(6秒)
    const float sceneChangeTime = 6.0f;
    // タイマー
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.Find("WaveManager");
        wave = waveManager.GetComponent<WaveManagement>();

        sceneManager = GameObject.Find("SceneManager");
        scene = sceneManager.GetComponent<GameSceneManager>();

        // scene上から演出用UIを見つける
        productUI = GameObject.Find("ProductUI");
        // ウェーブ情報(親)
        waveInfo = GameObject.Find("NextWaveInfo");
        // 撃退フェーズ
        waveIndex[0] = GameObject.Find("Info_EnemyWave");
        waveIndex[0].SetActive(false);
        // 対戦ウェーブ
        waveIndex[1] = GameObject.Find("Info_PlayerWave");

        productUI.SetActive(false);

        // アニメーター取得
        wallAnim = productUI.GetComponent<Animator>();


        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム開始したら
        if(scene.GetGameStartFlag())
        {
            GameUpdate();
        }
    }

    void GameUpdate()
    {


        // Waveがインターバルの時にUIをアクティブ化
        if (wave.wave == WaveManagement.WAVE_NUM.WAVE_INTERVAL)
        {
            // 前のウェーブがPVEウェーブならウェーブ情報を「対戦」にして、アニメーショントリガーをオン
            if (wave.tmpWave == WaveManagement.WAVE_NUM.WAVE_1_PVE || wave.tmpWave == WaveManagement.WAVE_NUM.WAVE_3_PVE)
            {
                waveIndex[1].SetActive(true);
                waveIndex[0].SetActive(false);

                wallAnim.SetTrigger("Panel");
            }
            // 前のウェーブがPVPウェーブなら、ウェーブ情報を「撃退」にして、アニメーショントリガーをオン
            else if (wave.tmpWave == WaveManagement.WAVE_NUM.WAVE_2_PVP || wave.tmpWave == WaveManagement.WAVE_NUM.WAVE_4_PVP)
            {
                waveIndex[0].SetActive(true);
                waveIndex[1].SetActive(false);

                wallAnim.SetTrigger("Panel");
            }

            wallAnim.SetBool("Interval", true);
            productUI.SetActive(true);
            waveInfo.SetActive(true);
        }
        else
        {
            wallAnim.SetBool("Interval", false);
            productUI.SetActive(false);
            waveInfo.SetActive(false);
        }


        // チュートリアル後の「撃退ウェーブ」表示
        if (wave.tmpTimer <= 1.0f && wave.wave == WaveManagement.WAVE_NUM.WAVE_TUTORIAL)
        {
            productUI.SetActive(true);
            waveInfo.SetActive(true);
            waveIndex[0].SetActive(true);

            wallAnim.SetTrigger("1stBattle");

            Debug.Log("撃退ウェーブ表示");

            AnimatorStateInfo stateInfo = wallAnim.GetCurrentAnimatorStateInfo(0);
            if(stateInfo.normalizedTime < 1.0f)
            {

            }
            else
            {
                productUI.SetActive(false);
                waveInfo.SetActive(false);
                waveIndex[0].SetActive(false);
            }
        }
        
    }

    // インターバル中の演出
    public void PlayIntervalProduct()
    {
        if(timer == 0.0f)
        {
            this.gameObject.SetActive(true);
        }

        // 一秒ごとにタイマーを加算
        timer += 1.0f * Time.deltaTime;

    }

    // チュートリアル演出用関数
    public bool PlayTutorialProduct(bool in_active)
    {
        // チュートリアル中はウェーブ情報を隠しておく
        waveInfo.SetActive(false);

        // アクティブ化されたら演出UIをアクティブ化し、アニメーションを再生
        if (in_active == true)
        {
            productUI.SetActive(true);
            wallAnim.SetBool("Tutorial", true);
        }
        if(in_active == false)
        {
            productUI.SetActive(false);
        }

        timer += 1.0f * Time.deltaTime;

        // 0.8秒で次へ
        if (timer >= 0.8f)
        {
            wallAnim.SetBool("Tutorial", false);
            return true;
        }
        else
        {
            return false;
        }

    }

    public void PlayWallAnim()
    {
        productUI.SetActive(true);
    }

    public void InitTimer()
    {
        timer = 0.0f;
    }

}
