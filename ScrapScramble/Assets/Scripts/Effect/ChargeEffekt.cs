using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ChargeEffekt : MonoBehaviour
{
    public EffekseerEmitter Effekseer;
    public GameObject chargeParticle;
    public Transform chagePoints;
    PlayerMovement playerMovement;
    Vector3 playePos;
    bool isParticlePlay;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playePos = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        Charge();
      
    }
    void Charge()
    {
        if (playerMovement.chargeFlg == true /*&& isParticlePlay == false*/)
        {
          
                //isParticlePlay = true;
                GameObject expl = Instantiate(chargeParticle, chagePoints) as GameObject;
                Destroy(expl, 3.5f);


        }
    }
}