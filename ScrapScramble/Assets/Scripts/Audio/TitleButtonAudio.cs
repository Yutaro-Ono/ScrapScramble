using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonAudio : MonoBehaviour
{

    public AudioClip sound;
    public GameObject Object;
    AudioSource audioSource;
    TitleSceneController titleSceneController;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        titleSceneController = Object.GetComponent<TitleSceneController>();
        audioSource.clip = sound;
    }

    // Update is called once per frame
    void Update()
    {
    
        if(titleSceneController.playSoundFlag==true)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
            
        
    }
}
