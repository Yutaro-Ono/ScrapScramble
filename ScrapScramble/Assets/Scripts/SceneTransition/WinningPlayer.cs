using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinningPlayer : MonoBehaviour
{
    // スコアが一番高いプレイヤー
    public int  winPlayerScore;
    //　プレイヤーの番号
    public int  PlayerNum;

    public Text winPlayerText;


    public int tiePlayerNum;
    public int tiePlayerNum2;
    public int tiePlayerNum3;
    void Start()
    {
        //　ニ秒間遅延させる
        Invoke("DecidingWinPlayer", 2.0f);
    }
    //　スコアが一番高いプレイヤーを決定する
    void DecidingWinPlayer()
    {
        for(int i=0;i<4;i++)
        {
            if(GameToResult.finalScore[i]>winPlayerScore)
            {
                winPlayerScore = GameToResult.finalScore[i];
                PlayerNum = i;
            }
        }
        for(int i=0;i<4; i++)
        {
            if (GameToResult.finalScore[i] == winPlayerScore)
            {
                if(PlayerNum!=i)
                {
                   
                   tiePlayerNum = i;
                    
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (winPlayerScore == GameToResult.finalScore[i])
            {
                if (PlayerNum != i && tiePlayerNum != i)
                {

                    tiePlayerNum2 = i;

                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (winPlayerScore == GameToResult.finalScore[i])
            {
                if (PlayerNum != i && tiePlayerNum != i && tiePlayerNum2 != i)
                {
                    tiePlayerNum3 = i;
                }
            }
        }

        if (PlayerNum == 0)
        {
            winPlayerText.text = "Player1Win";
        }
        if (PlayerNum == 1)
        {
            winPlayerText.text = "Player2Win";
        }
        if (PlayerNum == 2)
        {
            winPlayerText.text = "Player3Win";
        }
        if (PlayerNum == 3)
        {
            winPlayerText.text = "Player4Win";
        }

        if ( PlayerNum==0&& tiePlayerNum == 1)
            {
                winPlayerText.text = "Plaeyr1&2Win";
            }
            if ( PlayerNum == 0&&tiePlayerNum == 2)
            {
                winPlayerText.text = "Plaeyr1&3Win";
            }
            if (PlayerNum == 0 && tiePlayerNum == 3)
            {
                winPlayerText.text = "Plaeyr1&4Win";

            }
           if (PlayerNum == 1 && tiePlayerNum == 2)
            {
                winPlayerText.text = "Plaeyr2&3Win";
            }
            if (PlayerNum == 1 && tiePlayerNum == 3)
            {
                winPlayerText.text = "Plaeyr2&4Win";
            }
            if (PlayerNum == 2 && tiePlayerNum == 3)
            {
                winPlayerText.text = "Plaeyr3&4Win";
            }
            if (PlayerNum == 0 && tiePlayerNum == 2&&tiePlayerNum2==1)
            {
                winPlayerText.text = "Plaeyr1&2&3Win";
            }
            if (PlayerNum == 0 && tiePlayerNum == 3 && tiePlayerNum2 == 2)
            {
                winPlayerText.text = "Plaeyr1&3&4Win";
            }
            if (PlayerNum == 0 && tiePlayerNum == 3 && tiePlayerNum2 == 1)
            {
                winPlayerText.text = "Plaeyr1&2&4Win";
            }
            if (PlayerNum == 1 && tiePlayerNum == 3 && tiePlayerNum2 == 2)
            {
                winPlayerText.text = "Plaeyr2&3&4Win";
            }
          
                if (PlayerNum == 0 && tiePlayerNum == 3 && tiePlayerNum2 == 2 && tiePlayerNum3 == 1)
                {
                    winPlayerText.text = "Draw";
                }
            
        
      
        
    }

}
