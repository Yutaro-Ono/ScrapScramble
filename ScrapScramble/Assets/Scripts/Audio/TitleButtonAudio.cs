using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonAudio : MonoBehaviour
{
    Rewired.Player[] controllers = new Rewired.Player[PlayerManagement.playerNum];
    public AudioClip sound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            // その番号のプレイヤーがAIでなければ
            if (ChoiceMenuSceneController.getReady[i])
            {
                // 操作受付
                controllers[i] = Rewired.ReInput.players.GetPlayer(i);
            }
            // AIであれば
            else
            {
                controllers[i] = null;
            }
        }
        audioSource.clip = sound;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PlayerManagement.playerNum; ++i)
        {
            if (controllers[i].GetButton("A"))
            {
                audioSource.Play();
            }
        }
    }
}
