using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedScore : MonoBehaviour
{
    public Text  playerScoreText;
   

    PlayerStatus status;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
      
        StartCoroutine(ScoreAnimation(0, 1000, 2));
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
          

            playerScoreText.text = updateValue.ToString("f0"); // （"f0" の "0" は、小数点以下の桁数指定）
                                                                      // テキストの更新
            
            // 1フレーム待つ
            yield return null;

        } while (Time.time < endTime);
      
         // 最終的な着地のスコア

            playerScoreText.text = endScore.ToString();

        
    }
}
