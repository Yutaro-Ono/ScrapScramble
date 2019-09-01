using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiringEffect : MonoBehaviour
{
    public GameObject gtringEffect;
    public GameObject Player;
    PlayerInput input;
    Transform transfor;
    //PlayerStatus status;
    GameObject ins;
    bool isParticlePlay;
    // Start is called before the first frame update
    void Start()
    {
        transfor= Player.GetComponent<Transform>();
        input = Player.GetComponent<PlayerInput>();
        //status= Player.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GtringEffect()
    {
        if (input.GetWeaponAttackInput()&& isParticlePlay==false)
        {
            isParticlePlay = true;
            ins = Instantiate(gtringEffect, transfor.position, transfor.rotation) as GameObject;
        }
        if (!input.GetWeaponAttackInput())
        {
            Destroy(ins, 0.1f);
            isParticlePlay = false;
          
        }
    }
}
