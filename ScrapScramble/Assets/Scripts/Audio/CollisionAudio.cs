using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound1;
    public AudioClip sound2;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(sound1);
        }
        if (collision.gameObject.tag == "Wall")
        {
            audioSource.PlayOneShot(sound1);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            audioSource.PlayOneShot(sound1);
        }
        if (collision.gameObject.tag == "BulletOfEnemy")
        {
            audioSource.PlayOneShot(sound2);
        }
    }
}
