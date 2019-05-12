using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleToGame : MonoBehaviour
{
    //選択されたプレイ人数
    //staticによってシーン遷移後もこの変数は残る(TitleToGame.playerNumでアクセス可能)
    public static int playerNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneTransition(int in_playerNum)
    {
        playerNum = in_playerNum;
        SceneManager.LoadScene("BattleScene");
    }
}
