using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ChargeEffekt : MonoBehaviour
{
    public GameObject chargePlayer;
    public GameObject chargeParticle;
    public Transform chagePoints;
    PlayerMovement playerMovement;
    PlayerStatus status;
    GameObject expl;
    bool isParticlePlay;
    float initialScale;
    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = chargePlayer.GetComponent<PlayerMovement>();
        status = chargePlayer.GetComponent<PlayerStatus>();
        initialScale = chargeParticle.transform.localScale.x;
    }

    // Update is called once per frame
    private void Update()
    {
       
        Charge();
       
       
      
    }

    private void Charge()
    {

        if (playerMovement.chargeFlg == true && isParticlePlay == false)
        {

            isParticlePlay = true;
            expl = Instantiate(chargeParticle, chagePoints) as GameObject;
            EffectBigger();
        }
        if (playerMovement.chargeFlg == false)
        {
            Destroy(expl, 0.1f);
            isParticlePlay = false;
        }

    }

    void EffectBigger()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == status.armedStage)
            {
                float scale = initialScale + (5.0f * i);

                expl.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}