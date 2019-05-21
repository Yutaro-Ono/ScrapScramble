using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    // 弾のスピード
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        // 自分のlocalPositionに、
        // z方向のベクトルをspeed分だけ伸ばしたベクトルを足す
        // つまり、z方向にspeedずつ毎フレーム移動する
        //transform.localPosition += new Vector3(0, 0, 1.0f) * speed;

        // 自分のlocalPositionに、
        // 自分の前ベクトルをspeed分だけ伸ばしたベクトルを足す
        // つまり、前方向にspeedずつ毎フレーム移動する
        transform.localPosition += transform.forward * speed;
    }
}
