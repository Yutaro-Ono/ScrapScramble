//--------------------------------------------------------//
// Sprite画像用フェードイン・アウト演出
//       ※フェード処理したい画像にそのままアタッチ
//------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeSprite : MonoBehaviour
{
    public float alpha = 0f;                    // アルファ値

    private bool alphaSwitch;              

    Image spriteImg;        // Sprite画像格納用

    // フェード処理の種類
    public enum FadeType
    {
        NONE,               // 何もなし
        FADE_IN,            // フェードイン
        FADE_OUT,           // フェードアウト
        FADE_LOOP           // フェードイン→アウト ループ
    };

    // 上記列挙型の取得
    public FadeType fadeType = FadeType.NONE;

    // Start is called before the first frame update
    void Start()
    {
        spriteImg = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        FadeInOut();
    }

    public void FadeInOut()
    {

        if(alphaSwitch)
        {
            alpha += Time.deltaTime;
        }

        if(!alphaSwitch)
        {
            alpha -= Time.deltaTime;
        }

        SetAlpha();


        if(alpha >= 1)
        {
            alphaSwitch = false;
        }

        if(alpha <= 0)
        {
            alphaSwitch = true;
        }

    }

    void SetAlpha()
    {
        spriteImg.color = new Color(255, 255, 255, alpha);
    }

}
