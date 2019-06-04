using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJudgment : MonoBehaviour
{
    public GameObject dropObj;
    bool mutekiFlag;
    float mutekiTIme;
    float timeStep;
    public bool isAttackable;
    // 当たった時に呼ばれる関数
    public float lapseTime;

    void Start()
    {
        //isAttackableをtrueにしておく
        isAttackable = true;

        //lapseTimeを初期化
        lapseTime = 0.0f;
    }

    void LapseTime()
    {
        //isAttackableがfalseなら、直前のフレームからの経過時間を足す
        if (isAttackable == false)
        {
            lapseTime += Time.deltaTime;

            //lapsetimeが5秒を越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (lapseTime >= 1)
            {
                isAttackable = true;
                lapseTime = 0.0f;
            }
        }
    }
    private void Update()
    {
        LapseTime();
    }
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

            //エネミーに当たったらスクラップを落とす
            if (isAttackable == true)
            {
                if (other.gameObject.tag == "Enemy")

                {
                   
                    //d2.hp -= 1;
                    //　設定したアイテムをプレイヤーのの1m上から落とす
                    GameObject.Instantiate(dropObj, transform.position + Vector3.up, Quaternion.identity);
                    isAttackable = false;
                }
            }
        }
        
    }
}
