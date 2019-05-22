using System;
using UnityEngine;


public class PlayerMovememt : MonoBehaviour
{
    public float speed;          //プレイヤーの動くスピード
    public float atkSpeed;
    Vector3 Player_pos;          //プレイヤーのポジション
    private float moveX = 0f;            //x方向のImputの値
    private float moveZ = 0f;            //z方向のInputの値
    public float lapseTime;
    Rigidbody rb;
    private float chargeController;
    private float chargePower;
    bool coolTimeFlg;
    public bool chargePlayerStop;
    bool moveFlg;
    bool chargFlg;
    private void Start()
    {
     
        rb = GetComponent<Rigidbody>();
        Player_pos = GetComponent<Transform>().position; //最初の時点でのプレイヤーのポジションを取得
        chargePlayerStop = false;
        moveFlg = true;
        //isAttackableをtrueにしておく
        coolTimeFlg = true;
        chargFlg = false;
        //lapseTimeを初期化
        lapseTime = 0.0f;
    }

    void Stop()
    {
        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
    }
    void PushCharge()
    {
        if (coolTimeFlg==true)
        {
            if (Input.GetMouseButton(0))
            {
                chargeController += Time.deltaTime;
                chargePower += Time.deltaTime;
                chargePlayerStop = true;
                chargFlg = true;
            }
        }
        
        if (chargeController > 1.5f)
        {
            
            if (Input.GetMouseButtonUp(0))
            {
                rb.AddForce(transform.TransformDirection(Vector3.forward) * atkSpeed, ForceMode.Impulse);
             
                coolTimeFlg = false;
                chargePlayerStop = false;
                
            }

        }
     
        if (!Input.GetMouseButton(0))
        {
          
            chargePower = 0;
            chargeController = 0;
           chargePlayerStop = false;
        }

    }
    void PlayerStop()
    {
        rb.drag = 20;
    }
    void ChangDrag()
    {
        rb.drag = 0;
    }
    void InputKey()
    {
        if (Input.anyKey)
        {
            moveFlg = true;
        }
        else
        {
           
            moveFlg = false;
        }
    }
    void Update()
    {
        moveX = Input.GetAxis("Horizontal") ; //x方向のInputの値を取得
        moveZ = Input.GetAxis("Vertical"); //z方向のInputの値を取得

        Vector3 diff = transform.position - Player_pos;               //プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得

        if (diff.magnitude > 0.01f)                                   //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
        {
            transform.rotation = Quaternion.LookRotation(diff);       //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
        }
 
        Player_pos = transform.position;                              //プレイヤーの位置を更新
    }
    void CoolTime()
    {
        if (coolTimeFlg == false)
        {
            lapseTime += Time.deltaTime;

            //lapsetimeが5秒を越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (lapseTime >= 5)
            {
                chargFlg = false;
                coolTimeFlg = true;
                lapseTime = 0.0f;
            }
        }
    }
  
    void FixedUpdate()
    {

        PushCharge();
        CoolTime();
        Vector3 force = new Vector3(moveX * speed, 0.0f, moveZ * speed);  // 力を設定

        InputKey();
       
        rb.AddForce(force);
        if (moveFlg == false&&chargFlg==false)
        {
            Stop();
        }
        if (chargePlayerStop == true)
        {
            PlayerStop();
        }
        if (chargePlayerStop == false)
        {
            ChangDrag();
        }

        //rb.velocity = new Vector3(moveX, 0, moveZ);

    }

}