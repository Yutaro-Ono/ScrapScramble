using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEffect : MonoBehaviour
{
    public GameObject giant;
    public GameObject player;
    Transform giantPoints;
    PlayerStatus status;
    float initialScale;
    GameObject expl;
    int kazu;
    bool isParticlePlay;
    // Start is called before the first frame update
    void Start()
    {
        status.GetComponent<PlayerStatus>();
        giantPoints = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        GiantEfe();
    }
    void GiantEfe()
    {
       
            for (int i = 0; i < 4; i++)
            {
                if (i == status.armedStage)
                {
                   kazu = status.armedStage;
                   isParticlePlay = true;
                   

                }
            }
        if (kazu!=status.armedStage)
        {
            expl = Instantiate(giant, giantPoints) as GameObject;
            isParticlePlay = false;

        }
        if (isParticlePlay == false)
        {
            Destroy(expl, 0.4f);
        }
    }
}
