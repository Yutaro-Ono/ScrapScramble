//
// ScoreUIのFillメソッド管理
//
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGaugeSystem : MonoBehaviour
{
    public GameObject playerObj;

    PlayerStatus playerStatus;

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
        scoreGauge.fillAmount = playerStatus.score / 1000;
    }
}
