using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBgm : MonoBehaviour
{
    public GameObject player;
    public GameObject sud;
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;
    AudioSource audioSource2;
    PlayerStatus status;
    float WaveCount;
    float count;
    bool isAudioePlay;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource2 = sud.GetComponent<AudioSource>();
        status = player.GetComponent<PlayerStatus>();
        
        audioSource.clip = sound1;
        audioSource2.clip = sound2;
        audioSource.loop = true;
        audioSource2.loop = true;
        audioSource.volume = 0.7f;
        audioSource2.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        WaveCount++;
        count++;
        RingEnginAudio();
    }
    void RingEnginAudio()
    {
        WaveManagement.WAVE_NUM wave = status.GetWaveManager().wave;
       
        bool isVsEnemyWave = (wave == WaveManagement.WAVE_NUM.WAVE_1_PVE || wave == WaveManagement.WAVE_NUM.WAVE_3_PVE);
        if (isVsEnemyWave==false && isAudioePlay == true)
        {
           
            audioSource2.Pause();
            if (WaveCount > 590.0f)
            {
                audioSource.Play();
               
            }
            count = 0;
            isAudioePlay = false;
        }
        if (isVsEnemyWave==true && isAudioePlay == false)
        {
            audioSource.Pause();
           
            if(count>590.0f)
            {
                audioSource2.Play();
                
            }
            WaveCount = 0;

              isAudioePlay = true;

        }
    }
}
