using System;
using UnityEngine;


public class PlayerMovememt : MonoBehaviour
{
    public float speed;          //プレイヤーの動くスピード
    public float atkSpeed;
    Vector3 Player_pos;          //プレイヤーのポジション
    private float moveX = 0f;            //x方向のImputの値
    private float moveZ = 0f;            //z方向のInputの値
 
    Rigidbody rb;
    private float chargeController;
    private float chargePower;
    private void Start()
    {
     
        rb = GetComponent<Rigidbody>();
        Player_pos = GetComponent<Transform>().position; //最初の時点でのプレイヤーのポジションを取得
  
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
    

    void FixedUpdate()
    {



        Vector3 force = new Vector3(moveX*speed, 0.0f, moveZ*speed);  // 力を設定

        if (rb.velocity.magnitude < 40f)
        {
            rb.AddForce(force, ForceMode.Acceleration);
        }
        //rb.velocity = new Vector3(moveX, 0, moveZ);
    
        if (Input.GetMouseButton(0))
        { 
            chargeController += Time.deltaTime;

        }
        if (chargeController >1.5f)
        {
            if (Input.GetMouseButtonUp(0))
            {
                rb.AddForce(transform.TransformDirection(Vector3.forward) * atkSpeed, ForceMode.VelocityChange);

                chargePower = 0f;
                chargeController = 0;
            }

        }
        if (!Input.GetMouseButton(0))
        {
            chargePower = 0;
            chargeController = 0;
        }

    }

}