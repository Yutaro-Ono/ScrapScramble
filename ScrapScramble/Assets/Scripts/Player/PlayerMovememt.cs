using System;
using UnityEngine;


public class PlayerMovememt : MonoBehaviour
{
    public float speed;          //プレイヤーの動くスピード
    public float atkSpeed;
    Vector3 Player_pos;          //プレイヤーのポジション
    float moveX = 0f;            //x方向のImputの値
    float moveZ = 0f;            //z方向のInputの値
    Rigidbody rb;

    private void Start()
    {
        // 溜め攻撃用オブジェクト追加
        GameObject chargeAttackControllerObject = new GameObject("ChargeAttackController");
        chargeAttackControllerObject.transform.SetParent(transform);
        rb = GetComponent<Rigidbody>();
        ChargeAttackController chargeAttackController = chargeAttackControllerObject.AddComponent<ChargeAttackController>();
        chargeAttackController.Init(120, "Attack", () =>
        {
           

        });
    }
   

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");                          //x方向のInputの値を取得
        moveZ = Input.GetAxis("Vertical");                            //z方向のInputの値を取得
        Vector3 direction = new Vector3(moveX*speed, 0, moveZ*speed); //プレイヤーのRigidbodyに対してInputにspeedを掛けた値で更新し移動
       
        Vector3 diff = transform.position - Player_pos;               //プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得
                                                                      // チャージ時間をAnimatorにセット
       
        if (diff.magnitude > 0.01f)                                   //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
        {
            transform.rotation = Quaternion.LookRotation(diff);       //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
        }

        Player_pos = transform.position;                              //プレイヤーの位置を更新
    }
  
    void FixedUpdate()
    {
        rb.velocity = new Vector3(moveX*speed, 0, moveZ*speed);
    }
   
}