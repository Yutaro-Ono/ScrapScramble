using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginAudio : MonoBehaviour
{

    public GameObject sud;
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;
    AudioSource audioSource2;
    PlayerMovement move;
    bool isAudioePlay;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource2 = sud.GetComponent<AudioSource>();
        move = GetComponent<PlayerMovement>();
        audioSource.clip = sound1;
        audioSource2.clip = sound2;
        audioSource.loop = true;
        audioSource2.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        RingEnginAudio();
    }
    void RingEnginAudio()
    {
        if (move.moveFlag == true && isAudioePlay == true)
        {
            audioSource2.Pause();
          
            audioSource.Play();
          
            isAudioePlay = false;
        }
        if (move.moveFlag == false&&isAudioePlay==false)
        {
            audioSource.Pause();

            audioSource2.Play();
          
            isAudioePlay = true;
          
        }
    }
}
