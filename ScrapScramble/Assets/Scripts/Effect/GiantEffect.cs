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
                    float scale = initialScale + (5.0f * i);
              
                expl.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        //if (isParticlePlay == false)
        //{
        //    expl = Instantiate(giant, giantPoints) as GameObject;
        //    isParticlePlay = true;

        //}
        //if (isParticlePlay == true)
        //{
        //    Destroy(expl, 0.1f);
        //    isParticlePlay = false;
        //}
    }
}
