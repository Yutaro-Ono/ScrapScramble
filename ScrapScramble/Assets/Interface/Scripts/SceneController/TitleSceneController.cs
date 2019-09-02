using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    Rewired.Player[] players = new Rewired.Player[PlayerManagement.playerNum];

    // Start is called before the first frame update
    void Start()
    {
        // コントローラ情報取得
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            players[i] = Rewired.ReInput.players.GetPlayer(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 誰か一人でもAボタンを押せばフラグを真に
        bool anyonePressA = false;
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            anyonePressA = players[i].GetButton("A");

            // 押されればそれ以上の走査をしない
            if (anyonePressA)
            {
                break;
            }
        }

        if(Input.GetKey(KeyCode.Space) || anyonePressA)
        {
            SceneManager.LoadScene("ChoiceMenuScene");
        }
    }
}
