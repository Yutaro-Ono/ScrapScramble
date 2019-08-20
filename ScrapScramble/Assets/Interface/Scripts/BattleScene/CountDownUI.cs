//--------------------------------------------------------//
// カウントダウン用UI
//-------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : MonoBehaviour
{
    const float MAX_TIME = 4.0f;
    // タイマーオブジェクト
    GameObject timer;
    // テキスト格納用
    Text timerText;
    // トータルの時間
    public float totalTime;
    // 秒数
    int seconds;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.Find("Timer");
        timerText = timer.GetComponent<Text>();
        totalTime = MAX_TIME;
        seconds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CountDown();
    }

    // カウントダウンをする
    public void CountDown()
    {
        totalTime -= Time.deltaTime;
        seconds = (int)totalTime;
        timerText.text = seconds.ToString();
    }
}
