using System;
using UnityEngine;


public class PlayerMovememt : MonoBehaviour
{
    PlayerStatus status;
    PlayerInput input;

    public float speed;                  //プレイヤーの動くスピード
    public float atkSpeed;               //プレーヤーの攻撃スピード
    Vector3 Player_pos;                  //プレイヤーのポジション
    private float moveX = 0f;            //x方向のImputの値
    private float moveZ = 0f;            //z方向のInputの値
    public float lapseTime;              //チャージ攻撃のクールタイム
    Rigidbody rb;
    public float chargeController;       //溜時間
    public short chargePower;              //溜め時間での攻撃力変動
    bool finishedCoolTimeFlg;                    //クールタイムのフラグ
    public bool chargePlayerStop;        //プレーヤーが止まっているかどうか
    bool moveFlg;
    public bool chargeFlg;
    public bool timeFlg;

    // ドロップする資源オブジェクトのプレハブデータ
    GameObject resourcePrefab;

    // ドロップ時に資源を放る強さ
    const float dropAddForceStlength = 500.0f;

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
        //lapseTimeを初期化
        lapseTime = 0.0f;

        resourcePrefab = (GameObject)Resources.Load("Prefabs/Item/Resource/Resource");
        if (resourcePrefab == null)
        {
            Debug.Log("プレイヤー：資源プレハブの読み込みに失敗");
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
            if (Input.GetMouseButton(0) || input.GetTackleInput())
            {
                chargeController += Time.deltaTime;
                chargePower++;
                chargePlayerStop = true;
                chargeFlg = true;
            }
            else
            {
                // chargeFlgの更新
                chargeFlg = false;
            }
        }

        if (chargeController > 1.5f)
        {
            timeFlg = true;


        }
        if (timeFlg == true)
        {
            if (Input.GetMouseButtonUp(0) || input.GetTackleInputUp())

            {
                rb.AddForce(transform.TransformDirection(Vector3.forward) * atkSpeed, ForceMode.Impulse);

                finishedCoolTimeFlg = false;

                chargePlayerStop = false;
                chargeController = 0;
                timeFlg = false;
            }
        }
        if (!(Input.GetMouseButton(0) || input.GetTackleInput()))
        {

            chargePower = 0;
            chargeController = 0;
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
        if (input.GetHorizontalInput() != 0 || input.GetVerticalInput() != 0)
        {
            moveFlg = true;
            Debug.Log("プレイヤー" + (status.GetId() + 1) + "：縦方向操作入力");
        }
        else
        {

            moveFlg = false;
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

        /*
        moveX = Input.GetAxis("Horizontal"); //x方向のInputの値を取得
        moveZ = Input.GetAxis("Vertical"); //z方向のInputの値を取得
        */
        // 移動操作の受付
        /*
        // デバッグがしやすいようにパッドとキーボード両方の操作を受け付ける
        moveX = (input.GetHorizontalInput() + Input.GetAxis("Horizontal"));
        moveZ = (input.GetVerticalInput() + Input.GetAxis("Vertical"));
        */
        moveX = input.GetHorizontalInput();
        moveZ = input.GetVerticalInput();

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

        Vector3 diff = transform.position - Player_pos;               //プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得

        if (diff.magnitude > 0.01f)                                   //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
        {
            transform.rotation = Quaternion.LookRotation(diff);       //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
        }
        PushCharge();
        Player_pos = transform.position;                              //プレイヤーの位置を更新
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
            if (lapseTime >= 2)
            {
                chargeFlg = false;
            }
        }
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
                resource[i] = GameObject.Instantiate(resourcePrefab, gameObject.transform.position, Quaternion.identity);
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
        status.score -= (int)(dropMass * ResourceCollision.pointAddition);

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