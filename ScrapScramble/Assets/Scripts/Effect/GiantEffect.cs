using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEffect : MonoBehaviour
{
    public GameObject giant;
    Transform giantPoints;
    PlayerStatus status;
    GameObject expl;
    // Start is called before the first frame update
    void Start()
    {
        status=GetComponent<PlayerStatus>();
        giantPoints = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        GiantEfe();
    }
    void GiantEfe()
    {
       
           
        if (status.becomeBiggerEffectPlayTiming==true)
        {
            expl = Instantiate(giant, giantPoints) as GameObject;

        }
        if (status.becomeBiggerEffectPlayTiming ==false)
        {
            Destroy(expl, 1f);
        }
    }
}
