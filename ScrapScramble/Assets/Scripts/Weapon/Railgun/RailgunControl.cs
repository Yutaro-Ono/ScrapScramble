using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunControl : MonoBehaviour
{
    GameObject bulletPrefab;

    public bool droppedMode = true;

    [SerializeField]
    Vector3 droppedModeScale;

    //発射位置の位置データ
    Transform shootPoint;

    //発射間隔。一発撃ってから次撃てるようになるまでの秒数。
    public float shootInterval = 2.0f;

    float shootTimer;
    
    void Awake()
    {
        bulletPrefab = (GameObject)Resources.Load("Prefabs/Item/Weapon/Railgun/RailgunBullet");

        if (bulletPrefab == null)
        {
            Debug.Log("レールガン：弾のプレハブデータ取得に失敗");
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
    }

    private void OnEnable()
    {
        // クールタイム初期化
        shootTimer = shootInterval;
    }

    public void Attack()
    {
        //タイマーの値が発射間隔の値を超えていた場合、弾を発射する
        if (shootTimer >= shootInterval)
        {
            //発射
            GameObject instance = (GameObject)Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            //発射したプレイヤー情報を代入
            RailgunBulletMovement bulletScript = instance.GetComponent<RailgunBulletMovement>();
            bulletScript.SetShooterPlayer(transform.parent.parent.gameObject);

            //タイマーを0秒にリセット
            shootTimer = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && droppedMode)
        {
            PlayerStatus status = other.GetComponent<PlayerStatus>();

            status.EquipWeapon(Weapon.Railgun);

            Destroy(gameObject);
        }
    }
}
