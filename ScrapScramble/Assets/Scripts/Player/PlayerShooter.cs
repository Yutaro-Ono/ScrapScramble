using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // ショットとして発射するゲームオブジェクト.
    public GameObject shotObject;

    // ショットを生み出す位置を決定するゲームオブジェクト.
    public GameObject shotPosObject;

    // Update is called once per frame
    void Update()
    {
        // 左クリックが押されたらifの中に入る.
        if (Input.GetMouseButtonDown(0))
        {
            // ショットのゲームオブジェクトをコピーする.
            // コピーしたものを変数newShotにいれる.
            GameObject newShot = GameObject.Instantiate(shotObject);

            // newShotの位置をshotPosObjectと同じにする.
            newShot.transform.position = shotPosObject.transform.position;

            // newShotの向きをプレイヤーと同じにする
            newShot.transform.rotation = gameObject.transform.rotation;
        }
        if (Input.GetMouseButton(1))
        {
            // ショットのゲームオブジェクトをコピーする.
            // コピーしたものを変数newShotにいれる.
            GameObject newShot = GameObject.Instantiate(shotObject);

            // newShotの位置をshotPosObjectと同じにする.
            newShot.transform.position = shotPosObject.transform.position;

            // newShotの向きをプレイヤーと同じにする
            newShot.transform.rotation = gameObject.transform.rotation;
        }
    }
}
