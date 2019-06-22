using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingControl : MonoBehaviour
{
    GameObject bulletPrefab;

    //発射位置の位置データ
    Transform shootPoint;

    //発射間隔。一発撃ってから次撃てるようになるまでの秒数。
    public float shootInterval = 0.1f;

    float shootTimer;

    public bool droppedMode = true;

    [SerializeField]
    Vector3 droppedModeScale;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = (GameObject)Resources.Load("Prefabs/Item/Weapon/Gatling/GatlingBullet");

        if (bulletPrefab == null)
        {
            Debug.Log("ガトリング：弾のプレハブデータ取得に失敗");
        }

        shootPoint = transform.Find("Shoot Point");

        shootTimer = shootInterval;

        if (droppedMode)
        {
            gameObject.transform.localScale = droppedModeScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //タイマーのカウント
        shootTimer += Time.deltaTime;
        
        //とりあえずコマンド発動
        if (Input.GetKey(KeyCode.G) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStatus status = other.GetComponent<PlayerStatus>();

            status.EquipWeapon(Weapon.Gatling);
        }
    }
}
