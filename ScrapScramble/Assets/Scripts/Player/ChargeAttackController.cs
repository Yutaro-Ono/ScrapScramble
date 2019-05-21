using System;
using UnityEngine;

/// チャージ攻撃を制御するGameObject用コントローラクラス

public class ChargeAttackController : MonoBehaviour
{
   
    /// チャージ攻撃発動に必要なカウント
   
    protected int invoke_require_count;

   
    /// 現在のチャージカウント
  
    protected int current_count = 0;

   
    /// チャージ攻撃の溜めと発動を行うボタン
   
    protected string input_button_name;


    /// チャージ攻撃発動時に実行するアクション

    protected Action chargeAction;

    void Update()
    {
        // ボタンを離した際に一定値以上カウントが溜まっていればアクション実行
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (invoke_require_count <= current_count)
            {
                current_count = 0;
                chargeAction();
            }
        }

        // ボタン押下中はカウント加算
        if (Input.GetKey(KeyCode.Space))
        {
            current_count++;
        }
        else
        {
            // ボタンを離した場合はリセット
            current_count = 0;
        }
    }


    /// 初期化処理

    /// <param name="invoke_count"></param>
    /// <param name="input_button_name"></param>
    /// <param name="chargeAction"></param>
    public void Init(int invoke_count, string input_button_name, Action chargeAction)
    {
        this.invoke_require_count = invoke_count;
        this.input_button_name = input_button_name;
        this.chargeAction = chargeAction;
    }

 
    /// 現在のチャージカウントの溜め具合を0～1の間で返す

    /// <returns></returns>
    public float PropChargeCount()
    {
        // 既に溜まっている場合は1を返す
        if (invoke_require_count <= current_count)
        {
            return 1f;
        }
        // float型にキャストして割合を計算
        return (float)current_count / (float)invoke_require_count;
    }

}
