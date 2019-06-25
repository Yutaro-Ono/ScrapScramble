//------------------------------------------------------//
// ScoreUIのFillメソッド管理
//          GaugeUIにアタッチ
//----------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGaugeSystem : MonoBehaviour
{
    public GameObject playerObj;

    PlayerStatus playerStatus;

    float nextScore = 1000.0f;

    Image scoreGauge;

    // Start is called before the first frame update
    void Start()
    {
        scoreGauge = this.GetComponent<Image>();
        playerStatus = playerObj.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreGauge = this.GetComponent<Image>();

        // プレイヤーのスコアを参照し、ゲージを増減させる
        scoreGauge.fillAmount = (float)playerStatus.score / nextScore;

        //if (playerStatus.armedStage == 0)
        //{
        //    scoreGauge.fillAmount = (float)playerStatus.score / nextScore;
        //}

        //if (playerStatus.armedStage == 1)
        //{
        //    scoreGauge.fillAmount = (float)playerStatus.score / nextScore / 2.0f;
        //}

        //if (playerStatus.armedStage == 2)
        //{
        //    scoreGauge.fillAmount = (float)playerStatus.score / nextScore / 3.0f;
        //}

        //if (playerStatus.armedStage == 3)
        //{
        //    scoreGauge.fillAmount = (float)playerStatus.score / nextScore / 4.0f;
        //}

        //if (playerStatus.armedStage == 4)
        //{
        //    scoreGauge.fillAmount = (float)playerStatus.score / nextScore / 5.0f;
        //}
    }
}
