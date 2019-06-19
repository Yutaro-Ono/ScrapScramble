using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultResource : MonoBehaviour
{
    GameObject Player1;
    int sco = 1000;
    PlayerStatus script; 
    public GameObject Resource;
    float time = 0.1f;
   
    void Start()
    {

    }
    void ResourceGenerate()
    {
        float x = Random.Range(-70, -60);
       
        time -= Time.deltaTime;
        if (time <= 0)
        {
          
 
            if (sco % 50 == 0&&sco>0)
            {
              
                Vector3 CreatePoint = new Vector3(x,20,95);
                Instantiate(Resource, CreatePoint, Quaternion.identity);
                sco -= 50;
            }
           
            time = 0.1f;
        }
    }
    void Update()
    {
        ResourceGenerate();
     
    }
}
