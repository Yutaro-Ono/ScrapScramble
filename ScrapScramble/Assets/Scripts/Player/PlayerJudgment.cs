using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJudgment : MonoBehaviour
{

    public GameObject dropObj;
    bool mutekiFlag;
    //public int ItemNum;
    float mutekiTIme;
    float timeStep;
    public bool isAttackable;
    // 当たった時に呼ばれる関数
    public float lapseTime;
    public void DropScrap(int ScrapNumMin, int ScrapNumMax)
    {
        Vector3 dropPoint = new Vector3(3.0f, 2.0f, 3.0f); //飛ばす位置
        int ItemRandom = Random.Range(ScrapNumMin, ScrapNumMax);
        GameObject[] scrap = new GameObject[ItemRandom];
       
        for (int i = 0; i < ItemRandom; i++)
        {
            //　設定したアイテムをプレイヤーのの3m上から落とす
            scrap[i]= GameObject.Instantiate(dropObj, transform.position + dropPoint, Quaternion.identity);
        }
          
    }
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
    private void FixedUpdate()
    {
       
        LapseTime();
    }
    void OnCollisionEnter(Collision other)
    {
     
        PlayerMovement playerMove = GetComponent<PlayerMovement>();
        PlayerStatus playerStatus = GetComponent<PlayerStatus>();
        if (other.gameObject.tag == "Enemy")
        {
            EnemyStatus eneStatus = other.gameObject.GetComponent<EnemyStatus>();
            eneStatus.hitPoint -= (short)playerMove.tacklePower;
        }
    }
}
