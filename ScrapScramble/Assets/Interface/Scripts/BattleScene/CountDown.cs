//---------------------------------------------------------------------------//
// チュートリアル時のカウントダウンスクリプト
//
//-------------------------------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    const float MAX_TIME = 4.0f;

    // カウントダウン用のイメージを保管しているオブジェクト
    public GameObject[] timerImg = new GameObject[4];

    // カウントダウンオブジェクト
    GameObject countObj;
    Animator countAnim;

    // 経過した時間
    public float erapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        // 経過時間に最大時間を入れる
        erapsedTime = MAX_TIME;

        countObj = GameObject.Find("CountDownUI");

        countAnim = countObj.GetComponent<Animator>();

        for(int i = 0; i < 4; i++)
        {
            if(i == 0)
            {
                timerImg[i] = GameObject.Find("Count_Start").gameObject;
            }
            if(i == 1)
            {
                timerImg[i] = GameObject.Find("Count_1").gameObject;
            }
            if(i == 2)
            {
                timerImg[i] = GameObject.Find("Count_2").gameObject;
            }
            if(i == 3)
            {
                timerImg[i] = GameObject.Find("Count_3").gameObject;
            }
        }

        for(int i = 0; i < 4; i++)
        {
            timerImg[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 経過時間の初期化
    void InitErapsedTime()
    {
        erapsedTime = MAX_TIME;
    }

    // カウントダウン処理
    public float CountTimer()
    {
        erapsedTime -= Time.deltaTime;
        return erapsedTime;
    }

    // アニメーションのアクティブ化
    public void ActivateAnim(int in_num)
    {

        if(in_num == 0)
        {

            countAnim.SetBool("Anim_1", false);
            timerImg[1].SetActive(false);
            timerImg[0].SetActive(true);
            countAnim.SetBool("Anim_Start", true);
        }
        if(in_num == 1)
        {
  
            countAnim.SetBool("Anim_2", false);
            timerImg[2].SetActive(false);
            timerImg[1].SetActive(true);
            countAnim.SetBool("Anim_1", true);

        }
        if(in_num == 2)
        {

            countAnim.SetBool("Anim_3", false);
            timerImg[3].SetActive(false);
            timerImg[2].SetActive(true);
            countAnim.SetBool("Anim_2", true);

        }
        if(in_num == 3)
        {
            timerImg[3].SetActive(true);
            countAnim.SetBool("Anim_3", true);
        }
    }
}
