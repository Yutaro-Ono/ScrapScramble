using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiringEffect : MonoBehaviour
{
    public GameObject gtringEffect;
    public GameObject gtring;
    Transform transfor;
    GameObject ins;
    GatlingControl gatlingControl;
    bool isParticlePlay;

    void Start()
    {
        transfor= GetComponent<Transform>();
        gatlingControl = gtring.GetComponent<GatlingControl>();

    }

    void Update()
    {
        GtringEffect();
    }
    void GtringEffect()
    {
        if (gatlingControl.effectPlayTiming==true)
        {
            ins = Instantiate(gtringEffect, transfor) as GameObject;
        }
        if (gatlingControl.effectPlayTiming == false)
        {
            Destroy(ins, 0.22f);
        }
}
}
