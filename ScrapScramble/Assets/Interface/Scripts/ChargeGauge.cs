//---------------------------------------------------------------------------------//
// タックルのチャージ時間を視覚化するゲージシステム
// FollowUI内のプレイヤーごとに設置。playerObjには各プレイヤーのオブジェクトをアタッチ
//--------------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChargeGauge : MonoBehaviour
{
    // プレイヤー
    public GameObject playerObj;

    // タックルパワーの最大値(定数化しておきたい)
    float maxChargePower = 6.0f;
    // 現在のタックルパワー
    float nowChargePower;
    // ゲージを描画するかどうかのフラグ
    bool drawGaugeFlg;

    // チャージゲージの画像格納
    Image chargeGauge;

    // Start is called before the first frame update
    void Start()
    {
        chargeGauge = this.GetComponent<Image>();
        drawGaugeFlg = false;
        nowChargePower = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckChargeValue();

        if (drawGaugeFlg == true)
        {
            DrawGauge();
        }

    }

    // PlayerMovementクラスのチャージ関係の変数を監視する
    void CheckChargeValue()
    {
        PlayerMovememt moveScript = playerObj.GetComponent<PlayerMovememt>();

        drawGaugeFlg = moveScript.chargeFlg;

        // チャージフラグがオンの時現在のチャージパワーを加算、オフで初期化
        if (drawGaugeFlg == true)
        {
            nowChargePower = moveScript.tacklePower;
        }
        else
        {
            nowChargePower = 0.0f;
        }
    }

    // drawGaugeFlgがオンになっていたらゲージを描画
    void DrawGauge()
    {
        chargeGauge.fillAmount = nowChargePower / maxChargePower;
    }
}
