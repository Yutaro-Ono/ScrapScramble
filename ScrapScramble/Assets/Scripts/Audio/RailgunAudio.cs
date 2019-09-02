using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunAudio : MonoBehaviour
{
    public GameObject player;
    public AudioClip sound1;
    AudioSource audioSource;
    RailgunControl railgunControl;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        railgunControl = player.GetComponent<RailgunControl>();
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
            audioSource.PlayOneShot(sound1);
        }
    }
}
