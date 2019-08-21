using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedScore : MonoBehaviour
{

    public Text playerScoreText;

    //プレイヤーごとのスコア
    public int []finalScore ;
    public int PlayerNum;

    void Start()
    {


        StartCoroutine(ScoreAnimation(GameToResult.finalScore[PlayerNum], 2));
    }


    private IEnumerator ScoreAnimation( float Score,float time)
    {
        float startTime = Time.time;

        float endTime = startTime + time;

        do
        {

            float timeRate = (Time.time - startTime) / time;
            float updateValue = Score * timeRate;
            playerScoreText.text = updateValue.ToString("f0");
            yield return new WaitForSeconds(0.01f);

        } while (Time.time < endTime);
                playerScoreText.text = Score.ToString();
    }
}
