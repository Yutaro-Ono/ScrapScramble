using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJudgment : MonoBehaviour
{

    // 当たった時に呼ばれる関数
    void OnCollisionEnter(Collision other)
    {
        PlayerMovememt d1 = GetComponent<PlayerMovememt>();
        PlayerStatus d2 = GetComponent<PlayerStatus>();
        EnemyStatus d3 = other.gameObject.GetComponent<EnemyStatus>();
        if (d1.chargFlg == true)
        {
            

            if (other.gameObject.tag == "Enemy")
            {
                d3.hitPoint -= d2.atk;
                //Destroy(other.gameObject);
            }
         
        }
        if (d1.chargFlg == false)
        {
         
            // 敵に当たったら敵を消す.

            if (other.gameObject.tag == "Enemy")
            {
                d2.hp -= 1;
            }
        }
    }
}
