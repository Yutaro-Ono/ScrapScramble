//-------------------------------------------------------------------------//
// フェード処理スクリプト(Sprite・Text統合版)
//------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeScript : MonoBehaviour
{
    private float blinkSpeed = 0.5f;        // 点滅速度
    private float time = 0.0f;

    private Text textObj = null;
    private Image spriteObj = null;

    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;

        textObj = this.GetComponentInParent<Text>();
        spriteObj = this.GetComponentInParent<Image>();

        // nullの場合は子オブジェクトから検索
        if(textObj == null)
        {
            textObj = this.GetComponentInChildren<Text>();
        }
        if(spriteObj == null)
        {
            spriteObj = this.GetComponentInParent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(textObj != null)
        {
            textObj.color = GetAlphaColor(textObj.color);
        }
        if(spriteObj != null)
        {
            spriteObj.color = GetAlphaColor(spriteObj.color);
        }

    }

    // Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 3.0f * blinkSpeed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }
}
