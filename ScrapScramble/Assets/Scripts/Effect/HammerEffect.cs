using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEffect : MonoBehaviour
{
    public GameObject hammerEffect;
    public GameObject hammer;
    Transform transfor;
    GameObject ins;
    HammerControl hammerControl;
    // Start is called before the first frame update
    void Start()
    {
        transfor = GetComponent<Transform>();
        hammerControl = hammer.GetComponent<HammerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Hammer();
    }
    void Hammer()
    {
        if (hammerControl.effectPlayTiming == true)
        {
            ins = Instantiate(hammerEffect, transfor) as GameObject;
        }
        if (hammerControl.effectPlayTiming == false)
        {
            Destroy(ins, 0.5f);
        }
    }
}
