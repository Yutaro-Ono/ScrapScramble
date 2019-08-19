using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedScore : MonoBehaviour
{

    public Text [] playerScoreText;

    //プレイヤーごとのスコア
    int []finalScore ;

    void Start()
    {
        ScoreAnimation(300,2);
    }

    IEnumerator ScoreAnimation(float Score, float time)
    {
        
        //0fを経過時間にする
        float elapsedTime = 0.0f;
        float startTime = Time.time;
        // 終了時間
        float endTime = startTime + time;
        //timeが０になるまでループさせる
        do
        {
            float timeRate = (Time.time - startTime) / time;
           
        
            // テキストの更新
            for (int i = 0; i < 4; i++)
            {
                playerScoreText[i].text = (Score  * timeRate).ToString("f0");
            }

            elapsedTime += Time.deltaTime;
            // 0.01秒待つ
            yield return new WaitForSeconds(0.01f);
        } while (Time.time < endTime);
            // 最終的な着地のスコア
            for (int i = 0; i < 4; i++)
        {
            playerScoreText[i].text = Score.ToString();
        }
    }
}
