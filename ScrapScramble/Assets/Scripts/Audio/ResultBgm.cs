using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBgm : MonoBehaviour
{
 
    public AudioClip bgm;
    AudioSource audioSource;
    float bgmEnd;
    float bgmTim;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.Play();
        bgmEnd = 590.0f;
    }

    // Update is called once per frame
    void Update()
    {
        bgmTim++;
        if(bgmTim > bgmEnd)
        {
            audioSource.Stop();
        }
    }
}
