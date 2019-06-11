using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunControl : MonoBehaviour
{
    GameObject bulletPrefab;

    //発射位置の位置データ
    Transform shootPoint;

    //発射間隔。一発撃ってから次撃てるようになるまでの秒数。
    public float shootInterval = 3.0f;

    float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = (GameObject)Resources.Load("Prefabs/Item/Weapon/Railgun/RailgunBullet");

        if (bulletPrefab == null)
        {
            Debug.Log("レールガン：弾のプレハブデータ取得に失敗");
        }

        shootPoint = transform.Find("Shoot Point");

        shootTimer = shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマーのカウント
        shootTimer += Time.deltaTime;

        //とりあえずコマンド発動
        if (Input.GetKeyDown(KeyCode.R) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            Attack();
        }
    }

    public void Attack()
    {
        //タイマーの値が発射間隔の値を超えていた場合、弾を発射する
        if (shootTimer >= shootInterval)
        {
            //発射
            GameObject instance = (GameObject)Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            //タイマーを0秒にリセット
            shootTimer = 0.0f;
        }
    }
}
