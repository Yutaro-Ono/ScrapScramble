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

    private void Start()
    {
     
        rb = GetComponent<Rigidbody>();
        Player_pos = GetComponent<Transform>().position; //最初の時点でのプレイヤーのポジションを取得
  
    }
   

    void Update()
    {
        moveX = Input.GetAxis("Horizontal"); //x方向のInputの値を取得
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

        rb.velocity = new Vector3(moveX*speed, 0, moveZ * speed);

    }

}