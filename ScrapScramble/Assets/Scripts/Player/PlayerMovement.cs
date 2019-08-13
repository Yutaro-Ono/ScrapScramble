using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    PlayerStatus status;
    PlayerInput input;
    Rigidbody myRigidbody;

    Vector3 prevPos;
    
    // 移動関連
    float speed = 50.0f;
    float moveX = 0.0f;
    float moveZ = 0.0f;

    // 体当たり関連
    public bool chargeFlg = false;
    float chargeTimer = 0.0f;
    [SerializeField] bool tacklingFlag;
    public float tacklePower = 0.0f;
    bool coolTimeFlag = false;
    float coolTimer = 0.0f;
    const float CoolTime = 5.0f;

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
        myRigidbody = GetComponent<Rigidbody>();

        //最初の時点でのプレイヤーのポジションを取得
        prevPos = GetComponent<Transform>().position;

        tacklingFlag = false;

        resourcePrefab = (GameObject)Resources.Load("Prefabs/Item/Resource/Resource");
        if (resourcePrefab == null)
        {
            Debug.Log("プレイヤー：資源プレハブの読み込みに失敗");
        }
    }

    private void Update()
    {
        // 武器攻撃の受付
        CheckAttackCommand();

        // 移動操作受付
        CheckMoveCommand();

        // 体当たり操作受付
        CheckTackleCommand();

        // レイヤーの更新
        string layerName = tacklingFlag ? tacklingLayerName : normalLayerName;
        gameObject.layer = LayerMask.NameToLayer(layerName);

        // 進行方向を向く
        Vector3 positionDifference = transform.position - prevPos;
        if (positionDifference.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(positionDifference);
        }

        // 現フレームでの座標を記録
        prevPos = transform.position;
    }

    private void FixedUpdate()
    {
        CheckCoolTime();
        
        // 移動
        Vector3 force = new Vector3(moveX * speed, 0.0f, moveZ * speed);
        myRigidbody.AddForce(force);

        // 静止処理
        Brake();
    }

    void CheckAttackCommand()
    {
        if (input.GetWeaponAttackInput())
        {
            Attack();
        }
    }

    void CheckMoveCommand()
    {
        moveX = input.GetHorizontalInput();
        moveZ = input.GetVerticalInput();
    }

    void CheckTackleCommand()
    {
        // クールタイム中なら関数を抜ける
        if (coolTimeFlag)
        {
            return;
        }

        chargeFlg = input.GetTackleInput();
        if (chargeFlg)
        {
            // チャージタイマー加算
            chargeTimer += Time.deltaTime;

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
        }

        if (input.GetTackleInputUp())
        {
            // 体当たり実行
            myRigidbody.AddForce(transform.TransformDirection(Vector3.forward) * chargeTimer * tackleForceScalar, ForceMode.Impulse);

            // ダメージを与えられるレベルのチャージがされていた場合
            // 体当たり中フラグとクールタイム中フラグを真に
            tacklingFlag = coolTimeFlag = (chargeTimer >= 0.5f);

            // タイマー初期化
            chargeTimer = 0.0f;
        }
    }

    void CheckCoolTime()
    {
        // クールタイム処理
        if (coolTimeFlag)
        {
            // クールタイムのタイマー計算
            coolTimer += Time.deltaTime;

            // 体当たり中フラグの更新
            if (coolTimer >= 2.0f)
            {
                tacklingFlag = false;
            }

            // クールタイム終了判定＆処理
            if (coolTimer >= CoolTime)
            {
                coolTimeFlag = false;
                coolTimer = 0.0f;
            }
        }
    }

    void Brake()
    {
        bool moveFlag = (moveX != 0 || moveZ != 0);

        if ((!moveFlag && !tacklingFlag) || chargeFlg)
        {
            myRigidbody.velocity = Vector3.zero;
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