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

    // 演出用UI
    GameObject productUI;


    const float sceneChangeTime = 6.0f;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.Find("WaveManager");
        wave = waveManager.GetComponent<WaveManagement>();

        // scene上から演出用UIを見つける
        productUI = GameObject.Find("ProductUI");

        productUI.SetActive(false);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Waveがインターバルの時にUIをアクティブ化
        if(wave.wave == WaveManagement.WAVE_NUM.WAVE_INTERVAL)
        {
            productUI.SetActive(true);
        }
        else
        {
            productUI.SetActive(false);
        }
    }


    public void PlayIntervalProduct()
    {
        if(timer == 0.0f)
        {
            this.gameObject.SetActive(true);
        }

        // 一秒ごとにタイマーを加算
        timer += 1.0f * Time.deltaTime;

    }

}
