using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemEffect : MonoBehaviour
{
    public GameObject drop;
    public GameObject dropEffect;
    Transform transfor;
    GameObject ins;

    // Start is called before the first frame update
    void Start()
    {
     
        transfor = drop.GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        DropEffect();
    }
    void DropEffect()
    {


    }
}
