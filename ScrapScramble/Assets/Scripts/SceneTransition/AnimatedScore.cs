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
        
       
     
    }
    private void Update()
    {
        ScoreAnimation(GameToResult.finalScore[PlayerNum], 2);
    }
   
   public void ScoreAnimation( float Score,float time)
    {
        float startTime = Time.time;

        float endTime = startTime +Score;
        float elapsedTime = 0.0f;

        while (elapsedTime < endTime) 
        {
            float timeRate = (Time.time - startTime) / time;
            playerScoreText.text = (Score * timeRate).ToString();
            elapsedTime += Time.deltaTime;
        } 
               playerScoreText.text = Score.ToString();
    }
}
