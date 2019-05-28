using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveManagement : MonoBehaviour
{
    // 制限時間
    private const float limitTime = 30;

    // 制限時間表示用タイマー
    private float timer;

    // タイマー表示UI(テキスト)格納用
    public Text timerText;

    // 次のWAVEに進む際に立つフラグ
    private bool toNextWave;

    // WAVE番号管理
    public enum WAVE_NUM
    {
        WAVE_1_PVE = 0,
        WAVE_2_PVP,
        WAVE_3_PVE,
        WAVE_4_PVP
    }

    public WAVE_NUM wave;

    // Start is called before the first frame update
    void Start()
    {
        InitTime();
        wave = WAVE_NUM.WAVE_1_PVE;
    }

    // Update is called once per frame
    void Update()
    {

        if(toNextWave == true)
        {
            InitTime();
        }
        
        if(wave == WAVE_NUM.WAVE_1_PVE)
        {
            FirstWave();
        }

        if (wave == WAVE_NUM.WAVE_2_PVP)
        {
            SecondWave();
        }

        if (wave == WAVE_NUM.WAVE_3_PVE)
        {
            ThirdWave();
        }

        if (wave == WAVE_NUM.WAVE_4_PVP)
        {
            FourthWave();
        }


    }

    // タイマーに制限時間分を代入し初期化
    void InitTime()
    {
        timer = limitTime;
        toNextWave = false;
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

    // 第一ウェーブの処理
    void FirstWave()
    {
        ReduceTimer();
        ToStringTimer();
        // タイマーが0になったら次のウェーブに移動
        if (timer <= 0)
        {
            wave = WAVE_NUM.WAVE_2_PVP;
            toNextWave = true;
        }
    }

    // 第二ウェーブの処理
    void SecondWave()
    {
        ReduceTimer();
        ToStringTimer();
        // タイマーが0になったら次のウェーブに移動
        if (timer == 0)
        {
            toNextWave = true;
            wave = WAVE_NUM.WAVE_3_PVE;
        }
    }

    // 第三ウェーブ
    void ThirdWave()
    {
        ReduceTimer();
        ToStringTimer();
        // タイマーが0になったら次のウェーブに移動
        if (timer == 0)
        {
            toNextWave = true;
            wave = WAVE_NUM.WAVE_4_PVP;
        }
    }

    // 第四ウェーブ
    void FourthWave()
    {
        ReduceTimer();
        ToStringTimer();
    }
}
