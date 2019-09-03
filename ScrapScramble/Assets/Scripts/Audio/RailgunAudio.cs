using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunAudio : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;
    RailgunControl railgunControl;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        railgunControl = GetComponent<RailgunControl>();
        audioSource.volume =0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        RingRailgunAudio();
    }
    void RingRailgunAudio()
    {
        if(railgunControl.effectPlayTiming==true)
        {
            audioSource.Play();
        }
    }
}
