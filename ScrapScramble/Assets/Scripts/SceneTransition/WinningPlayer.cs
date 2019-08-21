using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinningPlayer : MonoBehaviour
{

   public int winPlayerScore;
   public int PlayerNum;

    public Text winPlayerText;

    // Start is called before the first frame update
    void Start()
    {
        DecidingWinPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DecidingWinPlayer()
    {
        for(int i=0;i<4;i++)
        {
            if(GameToResult.finalScore[i]> winPlayerScore)
            {
                winPlayerScore = GameToResult.finalScore[i];
                PlayerNum = i;
            }
        }
        winPlayerText.text = "プレイヤーの勝利";
    }
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
