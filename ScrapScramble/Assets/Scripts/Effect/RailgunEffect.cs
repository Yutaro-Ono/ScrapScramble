using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunEffect : MonoBehaviour
{
    public GameObject railgunEffect;
    public GameObject railgun;
    Transform transfor;
    GameObject ins;
    RailgunControl railgunControl;
    // Start is called before the first frame update
    void Start()
    {
        transfor = GetComponent<Transform>();
        railgunControl = railgun.GetComponent<RailgunControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Railgun();
    }
    void Railgun()
    {
        if (railgunControl.effectPlayTiming == true)
        {
            ins = Instantiate(railgunEffect, transfor) as GameObject;
        }
        if (railgunControl.effectPlayTiming == false)
        {
            Destroy(ins, 0.2f);
        }
    }
}
