using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    Rewired.Player[] players = new Rewired.Player[PlayerManagement.playerNum];
    
    bool prevButton;

    public bool playSoundFlag;

    // Start is called before the first frame update
    void Start()
    {
        // コントローラ情報取得
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            players[i] = Rewired.ReInput.players.GetPlayer(i);
        }

        prevButton = false;
        playSoundFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 誰か一人でもAボタンを押せばフラグを真に
        bool anyonePressDownA = false;
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            anyonePressDownA = players[i].GetButtonDown("A");

            // 押されればそれ以上の走査をしない
            if (anyonePressDownA)
            {
                break;
            }
        }

        // 前のフレームで押されていれば
        // 効果音再生のために一フレームずらす
        if(prevButton)
        {
            SceneManager.LoadScene("ChoiceMenuScene");
        }

        // フラグ記録
        if (Input.GetKeyDown(KeyCode.Space) || anyonePressDownA)
        {
            playSoundFlag = true;
            prevButton = true;
        }

        // エスケープが押されていればゲームを終了する関数
        CommonFunction.CheckEscapeForQuitApp();
    }
}
