using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveManagement : MonoBehaviour
{
    // バトルWAVEの制限時間
    private const float limitTime = 31.0f;
    // WAVE間のインターバルタイマー
    public const float intervalTime = 6.0f;

    // 制限時間カウント用タイマー
    private float timer;

    // タイマー表示UI(テキスト)格納用
    public Text timerText;

    // インターバルタイムに入る際に立つフラグ
    private bool toInterval;
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

    // Start is called before the first frame update
    void Start()
    {
        InitTime();
        wave = WAVE_NUM.WAVE_1_PVE;
    }

    // Update is called once per frame
    void Update()
    {

        if(toInterval == true)
        {
            EnterInterval();
        }

        if(toNextWave == true)
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


    }

    // タイマーに制限時間分を代入し初期化
    void InitTime()
    {
        timer = limitTime;

        // timer.textを表示
        timerText.enabled = true;

        toNextWave = false;
    }

    // WAVE終了時のインターバルタイムに入った時の処理
    void EnterInterval()
    {
        // timer.textを非表示
        timerText.enabled = false;

        timer = intervalTime;
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
        // タイマーが0になったら次のウェーブに移動
        if (timer <= 0)
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
        // タイマーが0になったら次のウェーブに移動
        if (timer <= 0)
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
        // タイマーが0になったら次のウェーブに移動
        if (timer <= 0)
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
    }

    void IntervalWave()
    {
        // WAVEテキストの表示

        // スタートテキストの表示



        ReduceTimer();

        if(timer <= 0)
        {
            wave = tmpWave++;
            toNextWave = true;
        }
    }
}
