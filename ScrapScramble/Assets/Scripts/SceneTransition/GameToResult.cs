﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameToResult : MonoBehaviour
{
    public static int[] finalScore = new int[4];
    GameObject player1Obj;
    GameObject player2Obj;
    GameObject player3Obj;
    GameObject player4Obj;

    WaveManagement waveManager;
    void Start()
    {
        player1Obj = GameObject.Find("Player1");
        player2Obj = GameObject.Find("Player2");
        player3Obj = GameObject.Find("Player3");
        player4Obj = GameObject.Find("Player4");

        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        // タイマーが0以下になったら次のシーンへ遷移
        if (waveManager.timer < 0)
        {
            SceneTransition();
        }

        // エスケープキーが押されていればゲームを終了する関数
        CommonFunction.CheckEscapeForQuitApp();
    }

    public void AssignScore()
    {
        PlayerStatus status1 = player1Obj.GetComponent<PlayerStatus>();
        PlayerStatus status2 = player2Obj.GetComponent<PlayerStatus>();
        PlayerStatus status3 = player3Obj.GetComponent<PlayerStatus>();
        PlayerStatus status4 = player4Obj.GetComponent<PlayerStatus>();
        finalScore[0] =status1.score;
        finalScore[1] = status2.score;
        finalScore[2] = status3.score;
        finalScore[3] = status4.score;

    }
    public void SceneTransition()
    {
        AssignScore();
        SceneManager.LoadScene("Ono_ResultScene");
    }
   
}
