using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ChargeEffekt : MonoBehaviour
{
    public GameObject chargeParticle;
    public Transform chagePoints;
    PlayerMovement playerMovement;
    PlayerStatus status;
    Vector3 playePos;
    GameObject expl;
    bool isParticlePlay;
    float initialScale;
    int prevScore;
    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        status = GetComponent<PlayerStatus>();
        playePos = GetComponent<Transform>().position;
        initialScale = chargeParticle.transform.localScale.x;
    }

    // Update is called once per frame
    private void Update()
    {
        if (prevScore != status.score)
        {
            EffectBigger();
        }
        Charge();
        prevScore = status.score;
    }

    private void Charge()
    {

        if (playerMovement.chargeFlg == true && isParticlePlay == false)
        {

            isParticlePlay = true;
            expl = Instantiate(chargeParticle, chagePoints) as GameObject;

        }
        if (playerMovement.chargeFlg == false)
        {
            Destroy(expl, 0.1f);
            isParticlePlay = false;
        }

    }

    void EffectBigger()
    {
        // 巨大化を行う
        for (int i = 0; i < 4; i++)
        {
            if (i == status.armedStage)
            {
                float scale = initialScale + (4.0f * i);

                chargeParticle.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}