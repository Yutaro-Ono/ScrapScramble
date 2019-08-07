using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    PlayerStatus status;
    PlayerInput input;

    public float speed;                  //プレイヤーの動くスピード
    public float atkSpeed;               //プレーヤーの攻撃スピード
    public int tacklePower;             //体当たりで与えるダメージ値
    Vector3 Player_pos;                  //プレイヤーのポジション
    private float moveX = 0f;            //x方向のInputの値
    private float moveZ = 0f;            //z方向のInputの値
    public float lapseTime;              //チャージ攻撃のクールタイム
    Rigidbody rb;
    public float chargeTimer;       //溜時間
    public short chargePower;              //溜め時間での攻撃力変動
    bool finishedCoolTimeFlg;                    //クールタイムのフラグ
    public bool chargePlayerStop;        //プレーヤーが止まっているかどうか
    bool moveFlg;
    public bool chargeFlg;
    public bool tackleReadyFlag;
    public bool tacklingFlag;

    const float tackleForceScalar = 100.0f;

    // ドロップする資源オブジェクトのプレハブデータ
    GameObject resourcePrefab;

    // ドロップ時に資源を放る強さ
    const float dropAddForceStlength = 500.0f;

    // 通常時のレイヤー名
    const string normalLayerName = "PlayerLayer";

    // 体当たり中のレイヤー名
    const string tacklingLayerName = "TacklingPlayerLayer";

    private void Start()
    {
        status = GetComponent<PlayerStatus>();
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        Player_pos = GetComponent<Transform>().position; //最初の時点でのプレイヤーのポジションを取得
        chargePlayerStop = false;
        moveFlg = false;
        finishedCoolTimeFlg = true;
        chargeFlg = false;
        tacklingFlag = false;
        //lapseTimeを初期化
        lapseTime = 0.0f;

        resourcePrefab = (GameObject)Resources.Load("Prefabs/Item/Resource/Resource");
        if (resourcePrefab == null)
        {
            Debug.Log("プレイヤー：資源プレハブの読み込みに失敗");
        }
    }

    void Update()
    {
        // 武器攻撃操作の受付
        if (input.GetWeaponAttackInput())
        {
            Debug.Log("武器攻撃ボタンが押された！");
            Attack();
        }


        //moveX = Input.GetAxis("Horizontal"); //x方向のInputの値を取得
        //moveZ = Input.GetAxis("Vertical"); //z方向のInputの値を取得

        // 移動操作の受付
        /*
        // デバッグがしやすいようにパッドとキーボード両方の操作を受け付ける
        moveX = (input.GetHorizontalInput() + Input.GetAxis("Horizontal"));
        moveZ = (input.GetVerticalInput() + Input.GetAxis("Vertical"));
        */
        moveX = input.GetHorizontalInput();
        moveZ = input.GetVerticalInput();
        if (moveX != 0 || moveZ != 0)
        {
            Debug.Log("Player" + status.GetId() + "に移動操作が入力された。");
        }

        /*
        // 移動操作の入力値をクランプ
        // パッドとキーボードの両対応のため、入力された値を足し合わせている
        // したがって大きすぎる値は調整しなければならない
        {
            // X
            if (moveX > 1.0f)
            {
                moveX = 1.0f;
            }
            else if (moveX < -1.0f)
            {
                moveX = -1.0f;
            }
            // Z
            if (moveZ > 1.0f)
            {
                moveZ = 1.0f;
            }
            else if (moveZ < -1.0f)
            {
                moveZ = -1.0f;
            }
        }
        */

        {
            Vector3 diff = transform.position - Player_pos;               //プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得

            if (diff.magnitude > 0.01f)                                   //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
            {
                transform.rotation = Quaternion.LookRotation(diff);       //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
            }
        }
        {
            /*
            //入力されている方向を向く
            Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

            if (moveDirection.magnitude > 0.01f)                                   //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);       //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
            }
            */
        }
        PushCharge();
        Player_pos = transform.position;                              //プレイヤーの位置を更新

        // レイヤーの更新
        gameObject.layer = LayerMask.NameToLayer((tacklingFlag) ? tacklingLayerName : normalLayerName);
    }

    void FixedUpdate()
    {
        CoolTime();
        Vector3 force = new Vector3(moveX * speed, 0, moveZ * speed);  // 力を設定

        InputKey();   //ボタンを押しているかどうか 

        rb.AddForce(force);  //プレイヤーに力を加える
        //プレイヤーを動かしていなかったら
        if (moveFlg == false && chargeFlg == false)
        {
            Stop();
        }
        //チャージ攻撃をしていたら
        if (chargePlayerStop == true)
        {
            PlayerStop();
        }
        //チャージ攻撃をしていなかったら
        if (chargePlayerStop == false)
        {
            ChangDrag();
        }
    }

    //プレーヤーの停止
    void Stop()
    {
        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
    }

    //チャージアタック処理
    void PushCharge()
    {
        if (finishedCoolTimeFlg == true)
        {
            //if (Input.GetMouseButton(0) || input.GetTackleInput())
            if (input.GetTackleInput())
            {
                chargeTimer += Time.deltaTime;
                chargePower++;
                chargePlayerStop = true;
                chargeFlg = true;

                tackleReadyFlag = true;
            }
            else
            {
                // chargeFlgの更新
                chargeFlg = false;
            }
        }

        // 0.5秒につき1ダメージ増加し、3秒で最大ダメージ値に
        for (int i = 0; i < (3.0f / 0.5f); i++)
        {
            if (chargeTimer >= 0.5f * (i + 1))
            {
                tacklePower = (short)(i + 1);
            }
            else
            {
                break;
            }
        }

        if (tackleReadyFlag == true)
        {
            if (Input.GetMouseButtonUp(0) || input.GetTackleInputUp())

            {
                rb.AddForce(transform.TransformDirection(Vector3.forward) * chargeTimer * tackleForceScalar, ForceMode.Impulse);

                // わずかにしか動かなかったタックルでクールタイムを取られるのは不憫なので、
                // 一定秒数未満のチャージしか行わなかった場合、クールタイムを免除する
                finishedCoolTimeFlg = (chargeTimer < 0.5f);

                chargePlayerStop = false;
                chargeTimer = 0;
                tackleReadyFlag = false;

                // クールタイムを課せられるほどチャージしていた場合は体当たりフラグを真に
                if (!finishedCoolTimeFlg)
                {
                    tacklingFlag = true;
                }
                // わずかにしか動けないほどのチャージしかしていなかった場合は、体当たり中とみなさない
                else
                {
                    tacklingFlag = false;
                }
            }
        }
        if (!(Input.GetMouseButton(0) || input.GetTackleInput()))
        {

            chargePower = 0;
            chargeTimer = 0;
            chargePlayerStop = false;
        }
    }

    //空気抵抗値の変更
    void PlayerStop()
    {
        rb.drag = 20;
    }
    void ChangDrag()
    {
        rb.drag = 0;
    }

    // 移動操作をしているかどうか
    void InputKey()
    {
        if (moveX != 0 || moveZ != 0)
        {
            moveFlg = true;
        }
        else
        {
            moveFlg = false;
        }
    }

    //クールタイム
    void CoolTime()
    {
        if (finishedCoolTimeFlg == false)
        {
            lapseTime += Time.deltaTime;

            //lapsetimeが5秒を越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (lapseTime >= 5)
            {

                finishedCoolTimeFlg = true;
                lapseTime = 0.0f;
            }

            //体当たり実行から二秒経過で体当たり中フラグを負に
            if (lapseTime >= 2)
            {
                tacklingFlag = false;
                tacklePower = 0;
            }
        }
    }

    // dropMass->排出する資源オブジェクトの個数
    // 返却値->最終的にドロップしたオブジェクトの数
    public uint DropResource(uint dropMass)
    {
        // もし指定数の資源オブジェクトを排出できるだけのスコアがない場合
        // 排出数をいじる
        if (status.score < dropMass * ResourceCollision.pointAddition)
        {
            // 1個でも持っているなら排出できるだけする
            if (status.score >= 1 * ResourceCollision.pointAddition)
            {
                dropMass = (uint)status.score / ResourceCollision.pointAddition;
            }
            // 1個も排出できないなら関数を抜ける
            else
            {
                return 0;
            }
        }

        // 排出する
        {
            //資源のインスタンス生成
            GameObject[] resource = new GameObject[dropMass];
            for (int i = 0; i < dropMass; i++)
            {
                resource[i] = GameObject.Instantiate(resourcePrefab, gameObject.transform.position + new Vector3(0, 50, 0), Quaternion.identity);
            }

            //資源を物理的に飛ばすため、資源ゲームオブジェクトのリジッドボディを取得
            Rigidbody[] resourceRigidbody = new Rigidbody[dropMass];
            for (int i = 0; i < dropMass; i++)
            {
                resourceRigidbody[i] = resource[i].GetComponent<Rigidbody>();
            }

            //資源を物理的に飛ばす
            for (int i = 0; i < dropMass; i++)
            {
                //飛ばす角度を計算
                float angle = Mathf.PI * 2 / dropMass * i;

                //飛ばす強さを設定
                //この時各方向の成分は取りうる最大値である
                //Vector3 dropDirection = new Vector3(1, Mathf.Sin(Mathf.PI / 4), 1) * dropAddForceStlength;
                Vector3 dropDirection = new Vector3(1, 0.001f, 1) * dropAddForceStlength;

                //角度に応じた値にx,z成分を設定する
                dropDirection.x *= Mathf.Sin(angle);
                dropDirection.z *= Mathf.Cos(angle);

                //ここでやっと飛ばす
                resourceRigidbody[i].AddForce(dropDirection);
            }
        }

        // 資源の減算
        status.score -= (int)(dropMass * ResourceCollision.pointAddition) * 2;

        return dropMass;
    }

    // 武器による攻撃関数
    void Attack()
    {
        // 現在装備している武器を取得
        // 装備していないなら直ちに関数を抜ける
        // できれば今後、継承を使った効率的なコードに書き換えたい。
        Weapon currentWeapon = status.GetCurrentWeapon();
        if (currentWeapon == Weapon.None)
        {
            return;
        }

        // ゲームオブジェクトを取得
        GameObject currentWeaponObj = status.GetWeaponObjectFromEnum(currentWeapon);
        if (currentWeaponObj == null)
        {
            Debug.Log("ゲームオブジェクトの取得に失敗");
            return;
        }

        // それぞれのスクリプトを取得して、攻撃関数を呼び出す
        // めんどくさいし初心者っぽい書き方だしメモリも食うでしょ？
        // だから今度時間ができたときに継承を使った書き方を試みます。
        if (currentWeapon == Weapon.Hammer)
        {
            HammerControl hammer = currentWeaponObj.GetComponent<HammerControl>();
            hammer.Attack();
        }
        else if (currentWeapon == Weapon.Gatling)
        {
            GatlingControl gatling = currentWeaponObj.GetComponent<GatlingControl>();
            gatling.Attack();
        }
        else if (currentWeapon == Weapon.Railgun)
        {
            RailgunControl railgun = currentWeaponObj.GetComponent<RailgunControl>();
            railgun.Attack();
        }
    }
}