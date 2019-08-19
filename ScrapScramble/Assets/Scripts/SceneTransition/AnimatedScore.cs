using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedScore : MonoBehaviour
{

    PlayerStatus status;

    public Text [] playerScoreText;

    //プレイヤーごとのスコア
    int finalScore ;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
       
        for (int i = 0; i < 4; i++)
        {
            finalScore= GameToResult.finalScore[i];
            StartCoroutine(ScoreAnimation(0, finalScore, 2));
        }
    }
   

    // スコアをアニメーションさせる
    private IEnumerator ScoreAnimation(float startScore, float endScore, float duration)
    {
        // 開始時間
        float startTime = Time.time;

        // 終了時間
        float endTime = startTime + duration;

        do
        {
            // 現在の時間の割合
            float timeRate = (Time.time - startTime) / duration;

            // 数値を更新
            float updateValue = (float)((endScore - startScore) * timeRate + startScore);
            for (int i = 0; i < 4; i++)
            {

                playerScoreText[i].text = updateValue.ToString("f0"); // （"f0" の "0" は、小数点以下の桁数指定）

            }                                                    // テキストの更新
            
            // 1フレーム待つ
            yield return null;

        } while (Time.time < endTime);

        // 最終的な着地のスコア
        for (int i = 0; i < 4; i++)
        {
            playerScoreText[i].text = endScore.ToString();
        }

        
    }
  
}
