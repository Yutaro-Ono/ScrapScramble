using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAudio : MonoBehaviour
{
    public GameObject waveManager;
    WaveManagement wave;
    AudioSource audioSource;
    public AudioClip sound;
    // Start is called before the first frame update
    void Start()
    {
        wave = waveManager.GetComponent<WaveManagement>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Change()
    {
        if (wave.timer <= 90)
        {
            audioSource.PlayOneShot(sound);
        }
        if (wave.timer <= 60)
        {
            audioSource.PlayOneShot(sound);
        }
        if (wave.timer <= 30)
        {
            audioSource.PlayOneShot(sound);
        }
        if (wave.timer <= 0)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
