using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAudio : MonoBehaviour
{
    public GameObject player;
    public AudioClip sound1;
    AudioSource audioSource;
    HammerControl hammerControl;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hammerControl = player.GetComponent<HammerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        RingHammerAudio();
    }
    void RingHammerAudio()
    {
        if (hammerControl.effectPlayTiming == true)
        {
            audioSource.PlayOneShot(sound1);
        }
    }
}
