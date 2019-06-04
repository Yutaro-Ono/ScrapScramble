using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int score;
    public int hp;
    public short atk;
    private float increase;
    public int armedStage;
    public short chargeAttackPower;
    bool getItem;
    bool nextBody;
    void Start()
    {
    
    }

    void ChargeAttack()
    {
        PlayerMovememt d1 = GetComponent<PlayerMovememt>();
        if (d1.chargePower <= 100)
        {
            chargeAttackPower = d1.chargePower;
        }
        if (d1.chargFlg==true)
        {
            atk = chargeAttackPower;
        }
       
    }
    void BodyBigger()
    {
        
        if (score>=100)
        {
            armedStage = 1;
        }
        if (score >= 200)
        {
            armedStage = 2;
        }
        if (score >= 300)
        {
            armedStage = 3;
        }
        if (armedStage == 1)
        {
            gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        if (armedStage == 2)
        {
            gameObject.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }
        if (armedStage == 3)
        {
            gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }
    }
    void PlayerDeath()
    {
        if(score<=0)
        {

            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        BodyBigger();
        ChargeAttack();
        PlayerDeath();
    }
}
