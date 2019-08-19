//--------------------------------------------------------//
// GameシーンにおけるWAVEの流れを管理
// シーンに一つだけ配置
//-------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveManagement : MonoBehaviour
{
    // バトルWAVEの制限時間
    public const float limitTime = 120.0f;
    // WAVE間のインターバルタイマー
    public const float intervalTime = 6.0f;

    // 制限時間カウント用タイマー
    public float timer;
    // インターバル時のタイマー
    public float intervalTimer;

    // タイマー表示UI(テキスト)格納用
    public Text timerText;

    // ウェーブ表示UI(テキスト)格納用
    public Text waveText;

    // インターバルタイムに入る際に立つフラグ
    public bool toInterval;
    // インターバルタイム終了後、次WAVEに進む際に立つフラグ
    private bool toNextWave;


    // WAVE番号管理
    public enum WAVE_NUM
    {
        WAVE_1_PVE = 0,
        WAVE_2_PVP,
        WAVE_3_PVE,
        WAVE_4_PVP,
        WAVE_INTERVAL
    }

    public WAVE_NUM wave;
    // 前回のWAVE番号の保管用
    public WAVE_NUM tmpWave;

    // テキスト表示enable
    public bool enableText;

    // WAVEデバッグ用 & WAVE番号
    public int waveCount;

    // Start is called before the first frame update
    void Start()
    {
        InitTime();
        timer = limitTime;
        wave = WAVE_NUM.WAVE_1_PVE;

        waveCount = 1;
        enableText = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (toInterval == true)
        {
            enableText = true;
            EnterInterval();
        }

        if (toNextWave == true)
        {
            InitTime();
        }

        // WAVE管理
        switch (wave)
        {
            case WAVE_NUM.WAVE_1_PVE:
                FirstWave();
                break;

            case WAVE_NUM.WAVE_2_PVP:
                SecondWave();
                break;

            case WAVE_NUM.WAVE_3_PVE:
                ThirdWave();
                break;

            case WAVE_NUM.WAVE_4_PVP:
                FourthWave();
                break;

            case WAVE_NUM.WAVE_INTERVAL:
                IntervalWave();
                break;

            default:
                break;
        }

        // WAVEのUI表示
        ToStringWave(enableText);

    }

    // タイマーに制限時間分を代入し初期化
    void InitTime()
    {
        intervalTimer = intervalTime;

        // timer.textを表示
        timerText.enabled = true;

        enableText = false;

        toNextWave = false;
    }

    // WAVE終了時のインターバルタイムに入った時の処理
    void EnterInterval()
    {
        // timer.textを非表示
        timerText.enabled = false;

        wave = WAVE_NUM.WAVE_INTERVAL;
        toInterval = false;
    }

    // timerを1秒につき1ずつ減らす
    void ReduceTimer()
    {
        timer -= Time.deltaTime;
    }

    // timerをUIのテキストに変換し表示
    void ToStringTimer()
    {
        timerText.text = ((int)timer).ToString();
    }

    void ToStringWave(bool enable)
    {
        if (enable == true)
        {
            waveText.text = "WAVE" + ((int)waveCount).ToString();
        }
        else
        {
            waveText.text = " ".ToString();
        }

    }

    //----------------------------------------------------------//
    //
    // WAVEごとの処理
    //
    //--------------------------------------------------------//

    // 第一ウェーブの処理
    void FirstWave()
    {
        ReduceTimer();
        ToStringTimer();

        if (waveCount == 1)
        {
            Debug.Log("第1ウェーブに入った");
            waveCount += 1;
        }


        // タイマーが0になったら次のウェーブに移動
        if (timer <= 90)
        {
            tmpWave = wave;
            toInterval = true;
        }
    }

    // 第二ウェーブの処理
    void SecondWave()
    {
        ReduceTimer();
        ToStringTimer();

        if (waveCount == 2)
        {
            Debug.Log("第2ウェーブに入った");
            waveCount += 1;
        }

        // タイマーが0になったら次のウェーブに移動
        if (timer <= 60)
        {
            tmpWave = wave;
            toInterval = true;
        }
    }

    // 第三ウェーブ
    void ThirdWave()
    {
        ReduceTimer();
        ToStringTimer();

        if (waveCount == 3)
        {
            Debug.Log("第3ウェーブに入った");
            waveCount += 1;
        }

        // タイマーが0になったら次のウェーブに移動
        if (timer <= 30)
        {
            tmpWave = wave;
            toInterval = true;
        }
    }

    // 第四ウェーブ
    void FourthWave()
    {
        ReduceTimer();
        ToStringTimer();

        if (waveCount == 4)
        {
            Debug.Log("第4ウェーブに入った");

        }

    }

    void IntervalWave()
    {
        // WAVEテキストの表示

        // スタートテキストの表示

        intervalTimer -= Time.deltaTime;

        // インターバルタイムが過ぎたら次WAVEに移行
        if (intervalTimer <= 0)
        {
            wave = ++tmpWave;
            toNextWave = true;
        }
    }


}
